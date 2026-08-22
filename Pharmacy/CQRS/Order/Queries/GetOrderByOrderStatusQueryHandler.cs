using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Order.Mapper;
using Pharmacy.CQRS.Order.Models.DTOs.Response;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Order.Queries;

public record GetOrderByOrderStatusQuery(
    long CustomerId,
    OrderStatus OrderStatus,
    int PageNumber,
    int PageSize) : IRequest<List<OrderResponseForCustomer>>;

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
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return OrderMappers.ToListOrderResponseForCustomers(orders);
    }
}