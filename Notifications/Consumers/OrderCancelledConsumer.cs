using MassTransit;
using Notifications.Services;
using Pharmacy.Event.Events;

namespace Notifications.Consumers;

public class OrderCancelledConsumer(
    INotificationService notificationService,
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
        await notificationService.ToCustomerOrderCancelled(
            message.Email,
            message.OrderId,
            message.UpdateTime,
            context.CancellationToken);
    }
}