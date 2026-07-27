using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Customer.Queries;

public record GetCustomerByIdQuery(
    long Id) : IRequest<CustomerResponse>;

public class GetCustomerByIdHandler(
    IDistributedCache cache,
    IMapper mapper,
    IApplicationDbContext dbContext)
    : IRequestHandler<GetCustomerByIdQuery, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var key = $"CustomerById-{request.Id}";
        var cachedCustomer = await cache.GetStringAsync(key, cancellationToken);

        if (cachedCustomer is not null)
        {
            var redis = JsonConvert.DeserializeObject<CustomerResponse>(cachedCustomer);

            if (redis is not null)
            {
                return redis;
            }
        }

        var customer = await dbContext.Customers
            .FindAsync(request.Id, cancellationToken);
        if (customer is null)
        {
            throw new RecourseNotFoundException("Customer not found");
        }

        var response = mapper.Map<CustomerResponse>(customer);
        await cache.SetStringAsync(
            key,
            JsonConvert.SerializeObject(response),
            new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            },
            cancellationToken);
        return response;
    }
}