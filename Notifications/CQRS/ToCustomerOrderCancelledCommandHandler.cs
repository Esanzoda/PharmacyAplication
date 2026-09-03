using MediatR;
using Notifications.Models;

namespace Notifications.CQRS;

public record ToCustomerOrderCancelledCommand(
    string ToEmail,
    long OrderId,
    DateTime CancelledAt) : IRequest;

public class ToCustomerOrderCancelledCommandHandler(
    IMediator mediator) : IRequestHandler<ToCustomerOrderCancelledCommand>
{
    public async Task Handle(ToCustomerOrderCancelledCommand request, CancellationToken cancellationToken)
    {
        var message = new EmailMessage
        {
            To = request.ToEmail,
            Subject = $"Cancelled Order",
            Body = $@"
                <h2>Your order is cancelled</h2>
                <p>Order Id: <strong>#{request.OrderId}</strong></p>
                <p>Order cancelled at <strong>{request.CancelledAt}</strong></p>  
            "
        };
        await mediator.Send(new SendToEmailCommand(message), cancellationToken);
    }
}