using MediatR;
using Notifications.Models;
using Pharmacy.Event.Events;

namespace Notifications.CQRS;

public record ToPharmacyExpiryProductCommand(
    string Email,
    DateTime Day,
    int Count,
    decimal TotalPurchasePrice,
    decimal TotalSalePrice,
    List<ExpiredItemEvent> ExpiryDateItems) : IRequest;

public class ToPharmacyExpiryProductCommandHandler(
    IMediator mediator) : IRequestHandler<ToPharmacyExpiryProductCommand>
{
    public async Task Handle(ToPharmacyExpiryProductCommand request, CancellationToken cancellationToken)
    {
        var itemsHtml = string.Join(
            "",
            request.ExpiryDateItems.Select(item => $"""
                                                    <tr>
                                                    <td>{item.ProductBatchId}</td>
                                                        <td>{item.ProductName}</td>
                                                        <td>{item.Quantity}</td>
                                                        <td>{item.TotalPurchasePrice:N2}</td>
                                                        <td>{item.TotalSalePrice:N2}</td>
                                                    </tr>
                                                    """)
        );
        var message = new EmailMessage
        {
            To = request.Email,
            Subject = "Expired Products",
            Body = $"""
                    <html>
                    <body>
                        <h2>Expired Products Report</h2>
                        <p><strong>Date:</strong> {request.Day}</p>
                        <p><strong>Total Count:</strong> {request.Count}</p>
                        <p><strong>Total Purchase Price:</strong> {request.TotalPurchasePrice:N2}</p>
                        <p><strong>Total Sale Price:</strong> {request.TotalSalePrice:N2}</p>
                        <h3>Products</h3>
                         <table border="1" cellpadding="5" cellspacing="0">
                     <thead>
                        <tr>
                             <th>Product Batch</th>
                              <th>Product Name</th>
                               <th>Quantity</th>
                                <th>Purchase Price</th>
                                 <th>Sale Price</th>
                         </tr>
                     </thead>
                     <tbody>{itemsHtml}</tbody>
                        </table>
                    </body>
                    </html>
                    """
        };
        await mediator.Send(new SendToEmailCommand(message), cancellationToken);
    }
}