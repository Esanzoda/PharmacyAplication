using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Purchase.Mapper;
using Pharmacy.Domain.Models.PurchaseEntity.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Purchase.Commands;

public record RemoveItemFromPurchaseCommand(
    long EmployeeId,
    long PharmacyId,
    long PurchaseId,
    long PurchaseItemId) : IRequest<PurchaseResponse>;

public class RemoveItemFromPurchaseCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<RemoveItemFromPurchaseCommand, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(
        RemoveItemFromPurchaseCommand request,
        CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.PurchaseId &&
                                      x.EmployeeEntityId == request.EmployeeId,
                cancellationToken);

        if (purchase == null)
        {
            throw new ResourceNotFoundException("Purchase not found");
        }

        var purchaseItemToRemove = await dbContext.PurchaseItems
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.PurchaseEntityId == request.PurchaseId &&
                                      x.Id == request.PurchaseItemId,
                cancellationToken);

        if (purchaseItemToRemove == null)
        {
            throw new ResourceNotFoundException("Purchase item not found");
        }

        var productBatch = await dbContext.ProductBatches
            .FirstOrDefaultAsync(x => x.PurchaseItemId == request.PurchaseItemId,
                cancellationToken);

        if (productBatch is null)
        {
            throw new ResourceNotFoundException("Product batch not found");
        }

        productBatch.IsActive = false;
        productBatch.Quantity -= purchaseItemToRemove.Quantity;
        productBatch.ProductEntity.Stock -= purchaseItemToRemove.Quantity;

        purchase.PurchaseItems.Remove(purchaseItemToRemove);
        purchase.TotalAmount = purchase.PurchaseItems.Sum(item => item.TotalPrice);

        await dbContext.SaveChangesAsync(cancellationToken);

        return PurchaseMappers.ToPurchaseResponse(purchase);
    }
}