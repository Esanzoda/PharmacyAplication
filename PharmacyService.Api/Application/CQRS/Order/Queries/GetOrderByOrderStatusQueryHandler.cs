using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Order.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Domain.Enums;

namespace PharmacyService.Api.Application.CQRS.Order.Queries;

public record GetOrderByOrderStatusQuery(
    long CustomerId,
    OrderStatus OrderStatus,
    PaginationRequest PaginationRequest) : IRequest<List<OrderResponseForCustomer>>;

public class GetOrderByOrderStatusQueryHandler(
    IApplicationDbContext dbContext) : IRequestHandler<GetOrderByOrderStatusQuery, List<OrderResponseForCustomer>>
{
    public async Task<List<OrderResponseForCustomer>> Handle(
        GetOrderByOrderStatusQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Where(x => x.CustomerEntityId == request.CustomerId &&
                        x.OrderStatus == request.OrderStatus)
            .OrderBy(x => x.Id)
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return OrderMappers.ToListOrderResponseForCustomers(orders);
    }
}