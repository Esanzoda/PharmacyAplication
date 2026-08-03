using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pharmacy.Controllers;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Deliver.Queries;

public record GetDeliverByIdQuery(
    long DeliverId) : IRequest<DeliverResponse>;

public class GetDeliverByIdHandler(
    IMapper mapper,
    IDistributedCache cache,
    IApplicationDbContext dbContext) : IRequestHandler<GetDeliverByIdQuery, DeliverResponse>
{
    public async Task<DeliverResponse> Handle(GetDeliverByIdQuery request, CancellationToken cancellationToken)
    {
        var key = $"DeliverById-{request.DeliverId}";
        var cached = await cache.GetStringAsync(key, cancellationToken);
        if (cached is not null)
        {
            var redis = JsonConvert.DeserializeObject<DeliverResponse>(cached);
            if (redis is not null)
            {
                return redis;
            }
        }

        var deliver = await dbContext.Delivers
            .FirstOrDefaultAsync(
                x => x.Id == request.DeliverId,
                cancellationToken);
        if (deliver is null)
        {
            throw new RecourseNotFoundException("Deliver not found");
        }

        var response = mapper.Map<DeliverResponse>(deliver);
        await cache.SetStringAsync(key,
            JsonConvert.SerializeObject(response),
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            }, cancellationToken);
        return response;
    }
}