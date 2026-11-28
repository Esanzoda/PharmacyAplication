using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Deliver.Mapper;
using PharmacyService.Api.Application.DTOs.Request;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Domain.Enums;

namespace PharmacyService.Api.Application.CQRS.Deliver.Queries;

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