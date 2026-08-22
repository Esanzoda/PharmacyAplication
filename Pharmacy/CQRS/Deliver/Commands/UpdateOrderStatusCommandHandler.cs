using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pharmacy.CQRS.Deliver.Models;
using Pharmacy.Event.Events;
using Pharmacy.Exception;
using Pharmacy.Interfaces;
using Pharmacy.Models.Domain.Enum;

namespace Pharmacy.CQRS.Deliver.Commands;

public record UpdateOrderStatusCommand(
    long DeliverId,
    long OrderId,
    DeliverUpdateOrderStatus NewOrderStatus) : IRequest<OrderStatus>;

public class UpdateOrderStatusCommandHandler(
    IApplicationDbContext dbContext,
    IPublishEndpoint publishEndpoint) : IRequestHandler<UpdateOrderStatusCommand, OrderStatus>
{
    public async Task<OrderStatus> Handle(
        UpdateOrderStatusCommand request,
        CancellationToken cancellationToken)
    {
        var deliver = await dbContext.Delivers
            .FindAsync([request.DeliverId],
                cancellationToken);
        if (deliver is null)
        {
            throw new ResourceNotFoundException("Deliver not found");
        }

        var order = await dbContext.Orders
            .Include(x => x.CustomerEntity)
            .FirstOrDefaultAsync(x => x.Id == request.OrderId &&
                                      (
                                          x.OrderStatus == OrderStatus.ReadyForPickup ||
                                          x.OrderStatus == OrderStatus.Shipped
                                      ) &&
                                      x.Deliver != null &&
                                      x.Deliver.Id == request.DeliverId,
                cancellationToken);
        if (order is null)
        {
            throw new ResourceNotFoundException("Order not found");
        }

        order.OrderStatus = (OrderStatus)request.NewOrderStatus;
        await dbContext.SaveChangesAsync(cancellationToken);
        if (order.OrderStatus == OrderStatus.Completed)
        {
            await publishEndpoint.Publish(new OrderCompletedEvent
            {
                Email = order.CustomerEntity.Email,
                OrderId = order.Id,
                TotalAmount = order.TotalAmount,
                CompletedAt = order.UpdateAt,
            }, cancellationToken);
        }
        else
        {
            await publishEndpoint.Publish(new OrderIsShippingEvent()
            {
                Email = order.CustomerEntity.Email,
                OrderId = order.Id,
                CustomerId = order.CustomerEntityId,
                TotalAmount = order.TotalAmount,
                DeliveryFee = order.DeliveryFee,
                ShippedAt = order.UpdateAt,
                DeliverName = deliver.Name
            }, cancellationToken);
        }

        return order.OrderStatus;
    }
}