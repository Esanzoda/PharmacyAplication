using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Customer.Models.DTOs.Response;
using Pharmacy.Exception;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Customer.Queries;

public record GetCustomerByPhoneNumberQuery(
    long PharmacyId,
    string PhoneNumber) : IRequest<CustomerResponse>;

public class GetCustomerByPhoneNumberQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext
) : IRequestHandler<GetCustomerByPhoneNumberQuery, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(GetCustomerByPhoneNumberQuery request,
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
            .ToDictionaryAsync(x => x.PhoneNumber, cancellationToken);


        if (!customers.TryGetValue(request.PhoneNumber, out var customer))
        {
            throw new RecourseNotFoundException("Customer not found");
        }


        return mapper.Map<CustomerResponse>(customer);
    }
}