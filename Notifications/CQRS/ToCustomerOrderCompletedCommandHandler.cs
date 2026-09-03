using MediatR;
using Notifications.Models;

namespace Notifications.CQRS;

public record ToCustomerOrderCompletedCommand(
    string ToEmail,
    long OrderId,
    decimal TotalAmount,
    DateTime CompletedAt) : IRequest;

public class ToCustomerOrderCompletedCommandHandler(
    IMediator mediator) : IRequestHandler<ToCustomerOrderCompletedCommand>
{
    public async Task Handle(ToCustomerOrderCompletedCommand request, CancellationToken cancellationToken)
    {
        var message = new EmailMessage
        {
            To = request.ToEmail,
            Subject = $"Order Completed",
            Body = $@"
                <h2>Your order is completed at<strong>{request.CompletedAt}</strong></h2>
                <p>Order id: <strong>#{request.OrderId}</strong></p>
                <p>Total amount: <strong>{request.TotalAmount:C}</strong></p>
                <p>Thanks for choose our  Pharmacy!</p>
            "
        };
        await mediator.Send(new SendToEmailCommand(message), cancellationToken);
    }
}