using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Purchase.Commands;

public record RemoveItemFromPurchaseCommand(
    long PharmacyId,
    long PurchaseId,
    long ItemId) : IRequest<PurchaseResponse>;

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
                                      x.Id == request.PurchaseId, cancellationToken);
        if (purchase == null)
        {
            throw new RecourseNotFoundException("Purchase not found");
        }

        var purchaseItemToRemove = await dbContext.PurchaseItems
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.PurchaseId == request.PurchaseId &&
                                      x.Id == request.ItemId,
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


        product.Stock -= purchaseItemToRemove.Quantity;

        purchase.PurchaseItems.Remove(purchaseItemToRemove);
       // dbContext.PurchaseItems.Remove(purchaseItemToRemove);
        purchase.TotalAmount = purchase.PurchaseItems.Sum(item => item.TotalPrice);

        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<PurchaseResponse>(purchase);
    }
}