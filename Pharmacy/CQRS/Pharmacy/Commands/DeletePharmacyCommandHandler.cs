using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Pharmacy.Commands;

public record DeletePharmacyCommand(
    long PharmacyId) : IRequest<bool>;

public class DeletePharmacyCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<DeletePharmacyCommand, bool>
{
    public async Task<bool> Handle(DeletePharmacyCommand request, CancellationToken cancellationToken)
    {
        var pharmacy = await dbContext.Pharmacies
            .FindAsync([request.PharmacyId],
                cancellationToken);
        if (pharmacy == null)
        {
            throw new ResourceNotFoundException("Pharmacy not found");
        }

        dbContext.Pharmacies.Remove(pharmacy);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}