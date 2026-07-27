using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Deliver.Commands;

public record DeleteDeliverCommand(
    long PharmacyId,
    long Id) : IRequest<bool>;

public class DeleteDeliverHandler(
    IApplicationDbContext dbContext,
    IDistributedCache cache) : IRequestHandler<DeleteDeliverCommand, bool>
{
    public async Task<bool> Handle(DeleteDeliverCommand request, CancellationToken cancellationToken)
    {
        var deliver = await dbContext.Delivers
            .FirstOrDefaultAsync(
                x => x.Id == request.Id &&
                     x.PharmacyId == request.PharmacyId,
                cancellationToken);
        if (deliver is null)
        {
            throw new RecourseNotFoundException("Deliver not found");
        }

        dbContext.Delivers
            .Remove(deliver);
        await dbContext.SaveChangesAsync(cancellationToken);
        var key = $"DeliverById-{request.Id}";
        await cache.RemoveAsync(key, cancellationToken);

        return true;
    }
}