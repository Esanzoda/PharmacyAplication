using MassTransit;
using MediatR;
using Notifications.CQRS;
using Pharmacy.Event.Events;

namespace Notifications.Consumers;

public class OrderCompletedConsumer(
    IMediator mediator,
    ILogger<OrderCompletedConsumer> logger) : IConsumer<OrderCompletedEvent>
{
    public async Task Consume(ConsumeContext<OrderCompletedEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Order completed: OrderId={OrderId}",
            message.OrderId);
        await mediator.Send(new ToCustomerOrderCompletedCommand(
            message.Email,
            message.OrderId,
            message.TotalAmount,
            message.CompletedAt));
    }
}