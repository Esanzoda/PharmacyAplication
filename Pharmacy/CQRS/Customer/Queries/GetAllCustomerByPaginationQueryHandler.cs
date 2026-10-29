using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Customer.Mapper;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Customer.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Customer.Queries;

public record GetAllCustomerByPaginationQuery(
    long PharmacyId,
    PaginationRequest PaginationRequest) : IRequest<List<CustomerResponse>>;

public class GetAllCustomerByPaginationHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetAllCustomerByPaginationQuery, List<CustomerResponse>>
{
    public async Task<List<CustomerResponse>> Handle(
        GetAllCustomerByPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(x => x.CustomerEntity)
            .Where(x => x.PharmacyId == request.PharmacyId)
            .OrderBy(x => x.Id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var customerIds = orders
            .Select(x => x.CustomerEntityId)
            .ToList();

        var customers = await dbContext.Customers
            .Where(x => customerIds.Contains(x.Id))
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .ToListAsync(cancellationToken);

        return CustomerMappers.ToListCustomerResponse(customers);
    }
}