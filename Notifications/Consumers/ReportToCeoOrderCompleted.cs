using MassTransit;
using MediatR;
using Notifications.CQRS;
using Pharmacy.Event.Events;

namespace Notifications.Consumers;

public class ReportToCeoOrderCompleted(
    IMediator mediator,
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
        await mediator.Send(new ToCeoReportCompletedOrdersCommand(
            message.To,
            message.Day,
            message.Count,
            message.TotalAmount));
    }
}