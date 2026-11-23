using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using PharmacyService.Api.Application.Common;
using PharmacyService.Api.Application.Exception;

namespace PharmacyService.Api.Application.CQRS.Deliver.Commands;

public record DeleteDeliverCommand(
    long Id) : IRequest<string>;

public class DeleteDeliverHandler(
    IApplicationDbContext dbContext,
    IDistributedCache cache) : IRequestHandler<DeleteDeliverCommand, string>
{
    public async Task<string> Handle(
        DeleteDeliverCommand request,
        CancellationToken cancellationToken)
    {
        var deliver = await dbContext.Delivers
            .FindAsync([request.Id],
                cancellationToken);
        if (deliver is null)
        {
            throw new ResourceNotFoundException("Deliver not found");
        }

        dbContext.Delivers
            .Remove(deliver);

        await dbContext.SaveChangesAsync(cancellationToken);
        var key = $"DeliverById-{request.Id}";
        await cache.RemoveAsync(key, cancellationToken);

        return Message.Deleted;
    }
}