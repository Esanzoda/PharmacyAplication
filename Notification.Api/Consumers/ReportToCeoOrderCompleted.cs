using MassTransit;
using Notification.Api_.Application.Services;
using PharmacyService.Contract.Events;

namespace Notification.Api_.Consumers;

public class ReportToCeoOrderCompleted(
    ILogger<ReportToCeoOrderCompleted> logger,
    IEmailService emailService) : IConsumer<OrderCompletedEventReportToCeo>
{
    public async Task Consume(ConsumeContext<OrderCompletedEventReportToCeo> context)
    {
        var message = context.Message;
        logger.LogInformation(
            "At {Day} our pharmacy had {Count} completed orders with total amount {TotalAmount}.",
            message.Day,
            message.Count,
            message.TotalAmount);
        await emailService.ToCeoReportCompletedOrders(
            message.To,
            message.Day,
            message.Count,
            message.TotalAmount);
    }
}