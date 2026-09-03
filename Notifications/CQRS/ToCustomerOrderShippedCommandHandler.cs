using MediatR;
using Notifications.Models;

namespace Notifications.CQRS;

public record ToCustomerOrderShippedCommand(
    string ToEmail,
    long OrderId,
    decimal DeliveryFee,
    decimal TotalAmount,
    DateTime ShippedAt,
    string DeliverName) : IRequest;

public class ToCustomerOrderShippedCommandHandler(
    IMediator mediator) : IRequestHandler<ToCustomerOrderShippedCommand>
{
    public async Task Handle(ToCustomerOrderShippedCommand request, CancellationToken cancellationToken)
    {
        var message = new EmailMessage
        {
            To = request.ToEmail,
            Subject = $"Order Shipped",
            Body = $@"
                <h2>Your order shipped</h2>
                <p>order Id: <strong>#{request.OrderId}</strong></p>
                <p>DeliveryFee <strong>{request.DeliveryFee:C}</strong></p>
                <p>Total amount <strong>{request.TotalAmount:C}</strong></p>
        <p>Order shipped at <strong>{request.ShippedAt}</strong></p>
 <p>Deliver name is <strong>{request.DeliverName}</strong></p>
                <p>Thanks for  order in our Pharmacy</p>

            "
        };
        await mediator.Send(new SendToEmailCommand(message), cancellationToken);
    }
}