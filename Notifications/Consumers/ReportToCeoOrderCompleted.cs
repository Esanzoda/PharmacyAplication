using MassTransit;
using Notifications.Services;
using Pharmacy.Event.Events;

namespace Notifications.Consumers;

public class ReportToCeoOrderCompleted(
    INotificationService notificationService,
    ILogger<ReportToCeoOrderCompleted> logger) : IConsumer<OrderCompletedEventReportToCeo>
{
    public async Task Consume(ConsumeContext<OrderCompletedEventReportToCeo> context)
    {
        var message = context.Message;
        logger.LogInformation(
            "At {Day} our pharmacy had {Count} completed orders with total amount {TotalAmount}.",
            message.Day,
            message.Count,
            message.TotalAmount);

        await notificationService.ToCeoReportCompletedOrders(
            message.To,
            message.Day,
            message.Count,
            message.TotalAmount,
            context.CancellationToken);
    }
}