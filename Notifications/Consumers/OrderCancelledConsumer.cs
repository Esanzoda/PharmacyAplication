using MassTransit;
using MediatR;
using Notifications.CQRS;
using Pharmacy.Event.Events;

namespace Notifications.Consumers;

public class OrderCancelledConsumer(
    IMediator mediator,
    ILogger<OrderCancelledConsumer> logger) : IConsumer<OrderCancelledEvent>
{
    public async Task Consume(ConsumeContext<OrderCancelledEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Order cancelled: OrderId={OrderId}, CustomerId={CustomerId} UpdateTime={UpdateTime}",
            message.OrderId,
            message.CustomerId,
            message.UpdateTime);
        await mediator.Send(new ToCustomerOrderCancelledCommand(
            message.Email,
            message.OrderId,
            message.UpdateTime));
    }
}