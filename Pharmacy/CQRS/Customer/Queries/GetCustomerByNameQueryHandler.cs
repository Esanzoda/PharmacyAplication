using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Customer.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Customer.Queries;

public record GetCustomerByNameQuery(
    long PharmacyId,
    string Name) : IRequest<List<CustomerResponse>>;

public class GetCustomerByNameQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext
) : IRequestHandler<GetCustomerByNameQuery, List<CustomerResponse>>
{
    public async Task<List<CustomerResponse>> Handle(GetCustomerByNameQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Where(x => x.PharmacyId == request.PharmacyId)
            .OrderBy(x => x.Id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        var customerIds = orders
            .Select(x => x.CustomerId)
            .ToList();
        var customers = await dbContext.Customers
            .Where(x => customerIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Name, cancellationToken);
        var customersList = new List<Models.Customer>();
        foreach (var unused in customers)
        {
            if (!customers.TryGetValue(request.Name, out var customer))
            {
                throw new RecourseNotFoundException("Customer not found");
            }

            customersList.Add(customer);
        }


        return mapper.Map<List<CustomerResponse>>(customersList);
    }
}