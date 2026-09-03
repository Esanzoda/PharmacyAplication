using MediatR;
using Notifications.Models;

namespace Notifications.CQRS;

public record ToCeoReportCompletedOrdersCommand(
    string ToEmail,
    DateTime Day,
    int Count,
    decimal TotalAmount) : IRequest;

public class ToCeoReportCompletedOrdersCommandHandler(
    IMediator mediator) : IRequestHandler<ToCeoReportCompletedOrdersCommand>
{
    public async Task Handle(ToCeoReportCompletedOrdersCommand request, CancellationToken cancellationToken)
    {
        var message = new EmailMessage
        {
            To = request.ToEmail,
            Subject = $"Report",
            Body = $@"
                <h2>Report Completed order</h2>
                <p>Day:<strong>{request.Day}</strong></p>
                <p>Count:<strong>{request.Count}</strong> </p>
                <p>TotalAmount:<strong>{request.TotalAmount}</strong></p>"
        };
        await mediator.Send(new SendToEmailCommand(message), cancellationToken);
    }
}