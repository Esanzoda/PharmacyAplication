using MassTransit;
using Notification.Api_.Application.Services;
using PharmacyService.Contract.Events;

namespace Notification.Api_.Consumers;

public class OrderShippedConsumer(
    ILogger<OrderShippedConsumer> logger,
    IEmailService emailService) : IConsumer<OrderIsShippingEvent>
{
    public async Task Consume(ConsumeContext<OrderIsShippingEvent> context)
    {
        var message = context.Message;
        logger.LogInformation(
            $"Order shipped: OrderId={message.OrderId}, CustomerId={message.CustomerId}, DeliveryFee{message.DeliveryFee}" +
            $" Total={message.TotalAmount} , Customer{message.Email} , Deliver Name {message.DeliverName}");
        await emailService.ToCustomerOrderShipped(
            message.Email,
            message.OrderId,
            message.DeliveryFee,
            message.TotalAmount,
            message.ShippedAt,
            message.DeliverName);
    }
}