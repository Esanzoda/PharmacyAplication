using MassTransit;
using Notification.Api_.Application.Services;
using PharmacyService.Contract.Events;

namespace Notification.Api_.Consumers;

public class OrderCreatedConsumer(
    ILogger<OrderCreatedConsumer> logger,
    IEmailService emailService) : IConsumer<OrderCreatedEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            $"Order created: OrderId={message.OrderId}, CustomerId={message.CustomerId}, DeliveryFee{message.DeliveryFee}" +
            $" Total={message.TotalAmount} , CreatedAt={message.CreatedAt}, Address{message.Address} Customer{message.Email}");

        await emailService.ToCustomerOrderCreated(
            message.Email,
            message.OrderId,
            message.TotalAmount,
            message.DeliveryFee,
            message.CreatedAt
        );
    }
}