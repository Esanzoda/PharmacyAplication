using MassTransit;
using Notification.Api_.Application.Services;
using PharmacyService.Contract.Events;

namespace Notification.Api_.Consumers;

public class OrderCompletedConsumer(
    ILogger<OrderCompletedConsumer> logger,
    IEmailService emailService) : IConsumer<OrderCompletedEvent>
{
    public async Task Consume(ConsumeContext<OrderCompletedEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Order completed: OrderId={OrderId}",
            message.OrderId);
        await emailService.ToCustomerOrderCompleted(
            message.Email,
            message.OrderId,
            message.TotalAmount,
            message.CompletedAt);
    }
}