using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Deliver.Mapper;
using Pharmacy.CQRS.Order.Models.DTOs.Response;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Deliver.Queries;

public record GetOrdersByStatusReadyForPickupQuery(int PageNumber, int PageSize)
    : IRequest<List<OrderResponseForDeliver>>;

public class GetOrdersNotReserveQueryHandler(
    IApplicationDbContext dbContext)
    : IRequestHandler<GetOrdersByStatusReadyForPickupQuery, List<OrderResponseForDeliver>>
{
    public async Task<List<OrderResponseForDeliver>> Handle(
        GetOrdersByStatusReadyForPickupQuery request,
        CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Where(x => x.Deliver == null &&
                        x.OrderType == OrderType.Deliver)
            .OrderBy(x => x.Id)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return DeliverMappers.ToListReserveOrdersForDeliver(orders);
    }
}