using MassTransit;
using MediatR;
using Notifications.CQRS;
using Pharmacy.Event.Events;

namespace Notifications.Consumers;

public class OrderCreatedConsumer(
    IMediator mediator,
    ILogger<OrderCreatedConsumer> logger) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            $"Order created: OrderId={message.OrderId}, CustomerId={message.CustomerId}, DeliveryFee{message.DeliveryFee}" +
            $" Total={message.TotalAmount} , CreatedAt={message.CreatedAt}, Address{message.Address} Customer{message.Email}");

        await mediator.Send(new ToCustomerOrderCreatedCommand(
            message.Email,
            message.OrderId,
            message.TotalAmount,
            message.DeliveryFee,
            message.CreatedAt));
    }
}