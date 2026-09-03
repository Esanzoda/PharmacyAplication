using MediatR;
using Notifications.Models;

namespace Notifications.CQRS;

public record ToCustomerOrderCreatedCommand(
    string ToEmail,
    long OrderId,
    decimal TotalAmount,
    decimal DeliveryFee,
    DateTime CreatedAt) : IRequest;

public class ToCustomerOrderCreatedCommandHandler(
    IMediator mediator) : IRequestHandler<ToCustomerOrderCreatedCommand>
{
    public async Task Handle(ToCustomerOrderCreatedCommand request, CancellationToken cancellationToken)
    {
        var message = new EmailMessage
        {
            To = request.ToEmail,
            Subject = $"Order Created",
            Body = $@"
                <h2>Your order created</h2>
                <p>order Id: <strong>#{request.OrderId}</strong></p>
                <p>DeliveryFee <strong>{request.DeliveryFee:C}</strong></p>
                <p>Total amount <strong>{request.TotalAmount:C}</strong></p>
        <p>Order created at <strong>{request.CreatedAt}</strong></p>
                <p>Thanks for  order in our Pharmacy</p>

            "
        };
        await mediator.Send(new SendToEmailCommand(message), cancellationToken);
    }
}