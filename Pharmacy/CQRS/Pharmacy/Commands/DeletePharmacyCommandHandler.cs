using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Domain.Models.Base.Domain;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Pharmacy.Commands;

public record DeletePharmacyCommand(
    long PharmacyId) : IRequest<string>;

public class DeletePharmacyCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<DeletePharmacyCommand, string>
{
    public async Task<string> Handle(DeletePharmacyCommand request, CancellationToken cancellationToken)
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
        return Message.Deleted;
    }
}