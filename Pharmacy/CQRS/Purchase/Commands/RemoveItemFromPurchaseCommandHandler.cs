using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Purchase.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Purchase.Commands;

public record RemoveItemFromPurchaseCommand(
    long EmployeeId,
    long PharmacyId,
    long PurchaseId,
    long PurchaseItemId) : IRequest<PurchaseResponse>;

public class RemoveItemFromPurchaseCommandHandler(
    IApplicationDbContext dbContext,
    IMapper mapper)
    : IRequestHandler<RemoveItemFromPurchaseCommand, PurchaseResponse>
{
    public async Task<PurchaseResponse> Handle(RemoveItemFromPurchaseCommand request,
        CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.PurchaseId &&
                                      x.EmployeeId == request.EmployeeId,
                cancellationToken);
        if (purchase == null)
        {
            throw new RecourseNotFoundException("Purchase not found");
        }

        var purchaseItemToRemove = await dbContext.PurchaseItems
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.PurchaseId == request.PurchaseId &&
                                      x.Id == request.PurchaseItemId,
                cancellationToken);

        if (purchaseItemToRemove == null)
        {
            throw new RecourseNotFoundException("Purchase item not found");
        }

        var product = await dbContext.Products
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == purchaseItemToRemove.ProductId,
                cancellationToken);
        if (product == null)
        {
            throw new RecourseNotFoundException("Product not found");
        }

        var productBatch = await dbContext.ProductBatches
            .FirstOrDefaultAsync(x => x.PurchaseItemId == request.PurchaseItemId,
                cancellationToken);
        if (productBatch is null)
        {
            throw new RecourseNotFoundException("Product batch not found");
        }

        productBatch.IsActive = false;
        product.Stock -= purchaseItemToRemove.Quantity;

        purchase.PurchaseItems.Remove(purchaseItemToRemove);
        purchase.TotalAmount = purchase.PurchaseItems.Sum(item => item.TotalPrice);

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<PurchaseResponse>(purchase);
    }
}