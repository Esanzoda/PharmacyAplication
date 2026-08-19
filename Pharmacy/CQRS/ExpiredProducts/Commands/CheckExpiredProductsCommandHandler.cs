using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.ExpiredProducts.Models;
using Pharmacy.Event.Events;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.ExpiredProducts.Commands;

public record CreateExpiredProductsCommand : IRequest;

public class CheckExpiredProductsHandler(
    IApplicationDbContext dbContext,
    ILogger<CheckExpiredProductsHandler> logger,
    IPublishEndpoint publishEndpoint) : IRequestHandler<CreateExpiredProductsCommand>

{
    public async Task Handle(
        CreateExpiredProductsCommand request,
        CancellationToken cancellationToken)
    {
        var tomorrow = DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(1));
        var now = DateTime.UtcNow;

        var expiredProducts = await dbContext.ProductBatches
            .Include(x => x.ProductEntity)
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
            .ToList();

        var pharmacies = await dbContext.Pharmacies
            .Where(x => pharmacyIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        foreach (var pharmacy in pharmacyGroup)
        {
            if (!pharmacies.TryGetValue(pharmacy.Key, out var pharmacyInfo))
            {
                logger.LogWarning(
                    "Pharmacy {PharmacyId} not found.",
                    pharmacy.Key);

                continue;
            }

            var expiryDate = new ExpiryDateEntity()
            {
                PharmacyId = pharmacy.Key
            };
            var expiryProducts = new List<ExpiryDateItem>();

            foreach (var productBatch in pharmacy)
            {
                var item = new ExpiryDateItemsEntity
                {
                    PharmacyId = productBatch.PharmacyId,
                    ProductBatch = productBatch,
                    ProductBatchId = productBatch.Id,
                    TotalSalePrice = productBatch.Quantity * productBatch.ProductEntity.SalePrice,
                    TotalPurchasePrice = productBatch.Quantity * productBatch.PurchasePrice
                };

                expiryDate.ExpiryDateItemsList.Add(item);

                expiryDate.TotalSalePrice += item.TotalSalePrice;
                expiryDate.TotalPurchasePrice += item.TotalPurchasePrice;

                productBatch.IsActive = false;
                productBatch.ProductEntity.Stock -= productBatch.Quantity;
                productBatch.IsDeleted = true;
                var expiryProduct = new ExpiryDateItem
                {
                    ProductName = productBatch.Name,
                    ProductBatchId = item.ProductBatchId,
                    Quantity = productBatch.Quantity,
                    TotalPurchasePrice = item.TotalPurchasePrice,
                    TotalSalePrice = item.TotalSalePrice
                };

                expiryProducts.Add(expiryProduct);
            }

            await dbContext.ExpireDateProducts.AddAsync(expiryDate, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);


            logger.LogInformation(
                "Created expired products report with {Count} items.",
                expiryDate.ExpiryDateItemsList.Count);

            await publishEndpoint.Publish(new CheckExpiryDateProductEvent
            {
                To = pharmacyInfo.Email,
                Day = now,
                Count = expiryProducts.Count,
                TotalPurchasePrice = expiryDate.TotalPurchasePrice,
                TotalSalePrice = expiryDate.TotalSalePrice,
                ExpiryDateItems = expiryProducts
            }, cancellationToken);
        }
    }
}