using MediatR;
using PharmacyService.Api.Application.Common;
using PharmacyService.Api.Application.Exception;

namespace PharmacyService.Api.Application.CQRS.Pharmacy.Commands;

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