using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Customer.Models.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Customer.Queries;

public record GetCustomerByPhoneNumberQuery(
    long PharmacyId,
    string PhoneNumber,
    int PageNumber,
    int PageSize) : IRequest<CustomerResponse>;

public class GetCustomerByPhoneNumberQueryHandler(
    IMapper mapper,
    IApplicationDbContext dbContext) : IRequestHandler<GetCustomerByPhoneNumberQuery, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(
        GetCustomerByPhoneNumberQuery request,
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
                        x.PhoneNumber == request.PhoneNumber)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var customerList = customers.ToList();

        return mapper.Map<CustomerResponse>(customerList);
    }
}