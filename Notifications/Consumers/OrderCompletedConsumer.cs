using MassTransit;
using Notifications.Services;
using Pharmacy.Event.Events;

namespace Notifications.Consumers;

public class OrderCompletedConsumer(
    INotificationService notificationService,
    ILogger<OrderCompletedConsumer> logger) : IConsumer<OrderCompletedEvent>
{
    public async Task Consume(ConsumeContext<OrderCompletedEvent> context)
    {
        var message = context.Message;

        logger.LogInformation(
            "Order completed: OrderId={OrderId},  CustomerId={CustomerId}",
            message.OrderId,
            message.CustomerId);

        await notificationService.ToCustomerOrderCompleted(
            message.Email,
            message.OrderId,
            message.TotalAmount,
            message.CompletedAt, context.CancellationToken);
    }
}