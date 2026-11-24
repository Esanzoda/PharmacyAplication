using MediatR;
using Microsoft.EntityFrameworkCore;
using PharmacyService.Api.Application.CQRS.Deliver.Mapper;
using PharmacyService.Api.Application.DTOs.Response;
using PharmacyService.Api.Application.Exception;
using PharmacyService.Domain.Enums;

namespace PharmacyService.Api.Application.CQRS.Deliver.Commands;

public record ShippedOrderCommand(
    long OrderId,
    long DeliverId) : IRequest<OrderResponseForDeliver>;

public class ReserveOrderToDeliverCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<ShippedOrderCommand, OrderResponseForDeliver>
{
    public async Task<OrderResponseForDeliver> Handle(
        ShippedOrderCommand request,
        CancellationToken cancellationToken)
    {
        var deliver = await dbContext.Delivers
            .FindAsync([request.DeliverId],
                cancellationToken);

        var order = await dbContext.Orders
            .FirstOrDefaultAsync(x => x.Id == request.OrderId &&
                                      x.OrderStatus == OrderStatus.ReadyForPickup &&
                                      x.Deliver == null,
                cancellationToken);
        if (order is null)
        {
            throw new ResourceNotFoundException("Order already has reserved ");
        }

        order.Deliver = deliver;
        await dbContext.SaveChangesAsync(cancellationToken);

        return DeliverMappers.ToReserveOrderForDeliver(order);
    }
}