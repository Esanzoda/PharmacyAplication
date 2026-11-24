using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Customer.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;

namespace PharmacyService.Api.Application.CQRS.Customer.Queries;

public record GetCustomerByNameQuery(
    long PharmacyId,
    string Name,
    PaginationRequest PaginationRequest) : IRequest<List<CustomerResponse>>;

public class GetCustomerByNameQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetCustomerByNameQuery, List<CustomerResponse>>
{
    public async Task<List<CustomerResponse>> Handle(
        GetCustomerByNameQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Where(x => x.PharmacyId == request.PharmacyId)
            .OrderBy(x => x.Id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var customerIds = orders
            .Select(x => x.CustomerEntityId)
            .ToList();

        var customers = await dbContext.Customers
            .Where(x => customerIds.Contains(x.Id) &&
                        x.Name == request.Name)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return CustomerMappers.ToListCustomerResponse(customers);
    }
}