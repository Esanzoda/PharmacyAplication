using Grpc.Contract.Proto;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Expired;
using Pharmacy.Event.Events;
using ProductService.Exception;
using ProductService.Interfaces;
using DateOnly = System.DateOnly;


namespace ProductService.CQRS.Product.Commands;

public record CreateExpiredProductsCommand : IRequest;

public class CheckExpiredProductsHandler(
    IProductDbContext dbContext,
    ILogger<CheckExpiredProductsHandler> logger,
    PharmacyService.PharmacyServiceClient pharmacyServiceClient,
    IPublishEndpoint publishEndpoint) : IRequestHandler<CreateExpiredProductsCommand>

{
    public async Task Handle(
        CreateExpiredProductsCommand request,
        CancellationToken cancellationToken)
    {
        var tomorrow = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(1));
        var now = DateTime.UtcNow;

        var expiredProducts = await dbContext.ProductBatches
            .Where(x => x.ExpiryDate <= tomorrow &&
                        x.IsActive)
            .ToListAsync(cancellationToken);

        if (expiredProducts.Count == 0)
            return;

        var pharmacyGroup = expiredProducts
            .GroupBy(x => x.PharmacyId)
            .ToList();

        var pharmacyIds = pharmacyGroup
            .Select(x => x.Key)
            .ToHashSet();
        var productIds = expiredProducts
            .Select(x => x.ProductEntityId)
            .ToHashSet();
        var products = await dbContext.Products
            .Where(x => productIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id,
                cancellationToken: cancellationToken);

        var response = await pharmacyServiceClient.GetPharmaciesByIdsAsync(
            new GetPharmaciesByIdsRequest
            {
                PharmacyIds = { pharmacyIds }
            }
        );
        var pharmacies = response.Pharmacies
            .ToDictionary(x => x.Id);

        foreach (var pharmacy in pharmacyGroup)
        {
            if (!pharmacies.TryGetValue(pharmacy.Key, out var pharmacyInfo))
            {
                throw new ResourceNotFoundException("Pharmacy not found");
            }


            var expiryDate = new ExpiredProductsEntity
            {
                PharmacyId = pharmacy.Key
            };
            var expiryProductsEvent = new List<ExpiredItemEvent>();

            foreach (var productBatch in pharmacy)
            {
                if (!products.TryGetValue(productBatch.ProductEntityId, out var product))
                {
                    throw new ResourceNotFoundException("Product not found");
                }

                var item = new ExpiredItemsEntity
                {
                    PharmacyId = productBatch.PharmacyId,
                    ProductBatch = productBatch,
                    ProductBatchId = productBatch.Id,
                    TotalPurchasePrice = productBatch.Quantity * productBatch.PurchasePrice,
                    TotalSalePrice = productBatch.Quantity * product.SalePrice
                };

                expiryDate.ExpiryDateItemsList.Add(item);

                expiryDate.TotalPurchasePrice += item.TotalPurchasePrice;
                expiryDate.TotalSalePrice += item.TotalSalePrice;

                productBatch.IsActive = false;
                var expiryProductEvent = new ExpiredItemEvent
                {
                    ProductName = productBatch.Name,
                    ProductBatchId = item.ProductBatchId,
                    Quantity = productBatch.Quantity,
                    TotalPurchasePrice = item.TotalPurchasePrice,
                    TotalSalePrice = item.TotalSalePrice
                };

                expiryProductsEvent.Add(expiryProductEvent);
            }

            await dbContext.ExpireDateProducts.AddAsync(expiryDate, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);


            logger.LogInformation(
                "Created expired products report with {Count} items.",
                expiryDate.ExpiryDateItemsList.Count);

            await publishEndpoint.Publish(new CheckExpiredProductEvent
            {
                To = pharmacyInfo.Email,
                Day = now,
                Count = expiryProductsEvent.Count,
                TotalPurchasePrice = expiryDate.TotalPurchasePrice,
                TotalSalePrice = expiryDate.TotalSalePrice,
                ExpiryDateItems = expiryProductsEvent
            }, cancellationToken);
        }
    }
}