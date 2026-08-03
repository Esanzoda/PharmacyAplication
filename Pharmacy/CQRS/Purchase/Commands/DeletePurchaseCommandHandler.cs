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
    public async Task<bool> Handle(DeletePurchaseCommand request, CancellationToken cancellationToken)
    {
        var purchase = await dbContext.Purchases
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.Id && x.EmployeeId == request.EmployeeId,
                cancellationToken);
        if (purchase is null)
        {
            throw new RecourseNotFoundException($"Purchase with id {request.Id} not found");
        }

        dbContext.Purchases
            .Remove(purchase);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}