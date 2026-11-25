using MassTransit;
using Notification.Api_.Application.Services;
using PharmacyService.Contract.Events;

namespace Notification.Api_.Consumers;

public class OrderCancelledConsumer(
    ILogger<OrderCancelledConsumer> logger,
    IEmailService emailService) : IConsumer<OrderCancelledEvent>
{
    public async Task Consume(ConsumeContext<OrderCancelledEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Order cancelled: OrderId={OrderId}, CustomerId={CustomerId} UpdateTime={UpdateTime}",
            message.OrderId,
            message.CustomerId,
            message.UpdateTime);
        await emailService.ToCustomerOrderCancelled(
            message.Email,
            message.OrderId,
            message.UpdateTime);
    }
}