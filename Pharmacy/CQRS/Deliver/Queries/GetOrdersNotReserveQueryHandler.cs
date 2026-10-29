using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Deliver.Mapper;
using Pharmacy.Domain.Models.Base.Domain.Enum;
using Pharmacy.Domain.Models.Base.Dto.Request;
using Pharmacy.Domain.Models.Order.DTOs.Response;
using Pharmacy.Interfaces;

namespace Pharmacy.CQRS.Deliver.Queries;

public record GetOrdersByStatusReadyForPickupQuery(
    PaginationRequest PaginationRequest) : IRequest<List<OrderResponseForDeliver>>;

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
            .Skip((request.PaginationRequest.PageNumber - 1) * PaginationRequest.PageSize)
            .Take(PaginationRequest.PageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return DeliverMappers.ToListReserveOrdersForDeliver(orders);
    }
}