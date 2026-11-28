using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Purchase.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Application.Exception;
using ProductService.Contract.Grpc;

namespace PharmacyService.Api.Application.CQRS.Purchase.Commands;

public record AddItemToPurchaseCommand(
    long PharmacyId,
    long Id,
    PurchaseItemRequest Request) : IRequest<PurchaseResponse>;

public class AddItemToPurchaseCommandHandler(
    IApplicationDbContext dbContext,
    ProductService.Contract.Grpc.ProductService.ProductServiceClient productClient) : IRequestHandler<AddItemToPurchaseCommand, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(
        AddItemToPurchaseCommand request,
        CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .Include(x => x.PurchaseItems)
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.Id,
                cancellationToken);

        if (purchase is null)
        {
            throw new ResourceNotFoundException($"Purchase with this id not found");
        }

        var product = await productClient.GetProductWithIdAndPharmacyIdAsync(
            new ProductWithIdAndPharmacyIdRequest
            {
                Id = request.Request.ProductEntityId,
                PharmacyId = request.PharmacyId
            },
            cancellationToken: cancellationToken);

        if (product == null)
        {
            throw new ResourceNotFoundException($"Product not found");
        }

        long newPurchaseItemId = 0;
        decimal newPurchaseItemTotalPrice = 0;
        var existItem = purchase.PurchaseItems
            .FirstOrDefault(x => x.ProductEntityId == product.ProductId);

        if (existItem != null)
        {
            existItem.Quantity += request.Request.Quantity;
            existItem.TotalPrice = existItem.Quantity * request.Request.PurchasePrice;

            var productBatch = await productClient.UpdateProductBatchAsync(
                new AddItemToBatchRequest
                {
                    ProductId = product.ProductId,
                    PurchaseItemId = existItem.Id,
                    Quantity = existItem.Quantity,
                    TotalPrice = (double)existItem.TotalPrice
                },
                cancellationToken: cancellationToken);

            if (!productBatch.Success)
            {
                throw new BusinessException(productBatch.Message);
            }
        }
        else
        {
            var newPurchaseItem = PurchaseMappers.ToPurchaseItem(request.Request);
            newPurchaseItem.PurchaseEntityId = purchase.Id;
            newPurchaseItem.PharmacyId = request.PharmacyId;
            newPurchaseItem.ProductEntityId = product.ProductId;
            newPurchaseItem.TotalPrice = request.Request.Quantity * request.Request.PurchasePrice;
            purchase.PurchaseItems.Add(newPurchaseItem);
            newPurchaseItemId = newPurchaseItem.Id;
            newPurchaseItemTotalPrice = newPurchaseItem.TotalPrice;
        }

        purchase.TotalAmount = purchase.PurchaseItems.Sum(item => item.TotalPrice);

        await dbContext.SaveChangesAsync(cancellationToken);

        if (existItem == null)
        {
            var newProductBatch = new AddProductBatch
            {
                PharmacyId = request.PharmacyId,
                ProductId = product.ProductId,
                Country = (Country)request.Request.Country,
                Quantity = request.Request.Quantity,
                PurchaseItemId = newPurchaseItemId,
                ProductionDate = new ProductService.Contract.Grpc.DateOnly
                {
                    Year = request.Request.ProductionDate.Year,
                    Month = request.Request.ProductionDate.Month,
                    Day = request.Request.ProductionDate.Day
                },

                ExpiryDate = new ProductService.Contract.Grpc.DateOnly
                {
                    Year = request.Request.ExpiryDate.Year,
                    Month = request.Request.ExpiryDate.Month,
                    Day = request.Request.ExpiryDate.Day
                },

                PurchasePrice = (double)request.Request.PurchasePrice,
                TotalPrice = (double)newPurchaseItemTotalPrice
            };
            var updateStockResponse = await productClient.AddProductBatchesAsync(
                new ProductBatches
                {
                    Branches = { newProductBatch }
                },
                cancellationToken: cancellationToken);

            if (!updateStockResponse.Success)
            {
                throw new BusinessException(updateStockResponse.Message);
            }
        }

        return PurchaseMappers.ToPurchaseResponse(purchase);
    }
}