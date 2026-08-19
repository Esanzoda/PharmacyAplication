using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Purchase.Commands;

public record DeletePurchaseCommand(
    long PharmacyId,
    long EmployeeId,
    long Id) : IRequest<bool>;

public class DeletePurchaseCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<DeletePurchaseCommand, bool>
{
    public async Task<bool> Handle(
        DeletePurchaseCommand request,
        CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .Include(x => x.PurchaseItems)
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.Id && x.EmployeeEntityId == request.EmployeeId,
                cancellationToken);

        if (purchase is null)
        {
            throw new ResourceNotFoundException($"Purchase with id {request.Id} not found");
        }

        var purchaseItemIds = purchase.PurchaseItems
            .Select(x => x.Id)
            .ToList();

        var productBatches = await dbContext.ProductBatches
            .Where(x => purchaseItemIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (var productBatch in productBatches)
        {
            productBatch.IsActive = false;
        }


        dbContext.Purchases
            .Remove(purchase);

        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}