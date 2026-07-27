using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.Interfaces;
using Pharmacy.Models.Dto.Response;

namespace Pharmacy.CQRS.Customer.Queries;

public record GetAllCustomerByPaginationQuery(
    int PageNumber,
    int PageSize
) : IRequest<List<CustomerResponse>>;

public class GetAllCustomerByPaginationHandler(
    IMapper mapper,
    IApplicationDbContext dbContext)
    : IRequestHandler<GetAllCustomerByPaginationQuery, List<CustomerResponse>>
{
    public async Task<List<CustomerResponse>> Handle(GetAllCustomerByPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var customers = await dbContext.Customers
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return mapper.Map<List<CustomerResponse>>(customers);
    }
}