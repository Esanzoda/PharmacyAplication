using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Order.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;

namespace PharmacyService.Api.Application.CQRS.Order.Queries;

public record GetAllOrdersQuery(
    long CustomerId,
    PaginationRequest PaginationRequest) : IRequest<List<OrderResponseForCustomer>>;

public class GetAllOrdersQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetAllOrdersQuery, List<OrderResponseForCustomer>>
{
    public async Task<List<OrderResponseForCustomer>> Handle(
        GetAllOrdersQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(x => x.OrderItems)
            .Where(x => x.CustomerEntityId == request.CustomerId)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return OrderMappers.ToListOrderResponseForCustomers(orders);
    }
}