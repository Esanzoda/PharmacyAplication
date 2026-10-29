using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Customer.Mapper;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Customer.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Customer.Queries;

public record GetCustomerByPhoneNumberQuery(
    long PharmacyId,
    string PhoneNumber,
    PaginationRequest PaginationRequest) : IRequest<List<CustomerResponse>>;

public class GetCustomerByPhoneNumberQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetCustomerByPhoneNumberQuery, List<CustomerResponse>>
{
    public async Task<List<CustomerResponse>> Handle(
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
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return CustomerMappers.ToListCustomerResponse(customers);
    }
}