using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Employee.Queries;

public record GetEmployeeByIdQuery(
    long PharmacyId,
    long Id) : IRequest<EmployeeResponse>;

public class GetEmployeeByIdQueryHandler(
    IApplicationDbContext dbContext,
    IMapper mapper,
    IDistributedCache cache
) : IRequestHandler<GetEmployeeByIdQuery, EmployeeResponse>
{
    public async Task<EmployeeResponse> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var key = $"EmployeeById-{request.Id}";
        var cachedEmployee = await cache.GetStringAsync(key, cancellationToken);
        if (cachedEmployee is not null)
        {
            var redis = JsonConvert.DeserializeObject<EmployeeResponse>(cachedEmployee);

            if (redis is not null)
            {
                return redis;
            }
        }

        var employee = await dbContext.Employees
            .FirstOrDefaultAsync(x => x.PharmacyId == request.PharmacyId &&
                                      x.Id == request.Id,
                cancellationToken);

        if (employee is null)
        {
            throw new RecourseNotFoundException("Employee not found");
        }

        var response = mapper.Map<EmployeeResponse>(employee);
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