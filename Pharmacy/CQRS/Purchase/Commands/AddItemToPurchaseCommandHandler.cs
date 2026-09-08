using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Purchase.Mapper;
using Pharmacy.Domain.Models.PurchaseEntity.DTOs.Request;
using Pharmacy.Domain.Models.PurchaseEntity.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Purchase.Commands;

public record AddItemToPurchaseCommand(
    long PharmacyId,
    long Id,
    PurchaseItemRequest Request) : IRequest<PurchaseResponse>;

public class AddItemToPurchaseCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<AddItemToPurchaseCommand, PurchaseResponse>
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

        var product = await dbContext.Products
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.Request.ProductEntityId,
                cancellationToken);
        if (product == null)
        {
            throw new ResourceNotFoundException($"Product not found");
        }

        var existItem = purchase.PurchaseItems
            .FirstOrDefault(x => x.ProductEntityId == product.Id);
        if (existItem != null)
        {
            existItem.Quantity += request.Request.Quantity;
            existItem.TotalPrice = existItem.Quantity * request.Request.PurchasePrice;

            var productBatch = await dbContext.ProductBatches
                .FirstOrDefaultAsync(x => x.PurchaseItemId == existItem.Id,
                    cancellationToken);
            if (productBatch == null)
            {
                throw new ResourceNotFoundException($"Product Batch not found");
            }

            productBatch.Quantity += request.Request.Quantity;
            productBatch.TotalPurchasePrice = existItem.TotalPrice;
        }
        else
        {
            var purchaseItem = PurchaseMappers.ToPurchaseItem(request.Request);
            purchaseItem.PurchaseEntityId = purchase.Id;
            purchaseItem.PharmacyId = request.PharmacyId;
            purchaseItem.ProductEntityId = product.Id;
            purchaseItem.TotalPrice = request.Request.Quantity * request.Request.PurchasePrice;
            purchase.PurchaseItems.Add(purchaseItem);
        }

        product.Stock += request.Request.Quantity;

        purchase.TotalAmount = purchase.PurchaseItems.Sum(item => item.TotalPrice);

        await dbContext.SaveChangesAsync(cancellationToken);

        return PurchaseMappers.ToPurchaseResponse(purchase);
    }
}