using MassTransit;
using MediatR;
using Notifications.CQRS;
using Pharmacy.Event.Events;

namespace Notifications.Consumers;

public class OrderShippedConsumer(
    IMediator mediator,
    ILogger<OrderShippedConsumer> logger) : IConsumer<OrderIsShippingEvent>
{
    public async Task Consume(ConsumeContext<OrderIsShippingEvent> context)
    {
        var message = context.Message;
        logger.LogInformation(
            $"Order shipped: OrderId={message.OrderId}, CustomerId={message.CustomerId}, DeliveryFee{message.DeliveryFee}" +
            $" Total={message.TotalAmount} , Customer{message.Email} , Deliver Name {message.DeliverName}");
        await mediator.Send(new ToCustomerOrderShippedCommand(
            message.Email,
            message.OrderId,
            message.DeliveryFee,
            message.TotalAmount,
            message.ShippedAt,
            message.DeliverName));
    }
}