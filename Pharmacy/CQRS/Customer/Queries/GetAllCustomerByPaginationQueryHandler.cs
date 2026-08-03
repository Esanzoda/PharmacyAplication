using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Customer.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Customer.Queries;

public record GetAllCustomerByPaginationQuery(
    int PageNumber,
    int PageSize,
    long PharmacyId
) : IRequest<List<CustomerResponse>>;

public class GetAllCustomerByPaginationHandler(
    IMapper mapper,
    IApplicationDbContext dbContext)
    : IRequestHandler<GetAllCustomerByPaginationQuery, List<CustomerResponse>>
{
    public async Task<List<CustomerResponse>> Handle(GetAllCustomerByPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(x => x.Customer)
            .Where(x => x.PharmacyId == request.PharmacyId)
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        var customerIds = orders
            .Select(x => x.CustomerId)
            .ToList();
        var customers = await dbContext.Customers
            .Where(x => customerIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);
        var customersList = new List<Models.Customer>();
        foreach (var customerId in customers)
        {
            if (!customers.TryGetValue(customerId.Key, out var customer))
            {
                throw new RecourseNotFoundException("Customer not found");
            }

            customersList.Add(customer);
        }

        return mapper.Map<List<CustomerResponse>>(customersList);
    }
}