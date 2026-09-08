using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Customer.Mapper;
using Pharmacy.Domain.Models.Customer.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Customer.Queries;

public record GetCustomerByNameQuery(
    long PharmacyId,
    string Name,
    int PageNumber,
    int PageSize) : IRequest<List<CustomerResponse>>;

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
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return CustomerMappers.ToListCustomerResponse(customers);
    }
}