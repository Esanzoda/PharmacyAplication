using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Notifications.Models;
using Notifications.Setting;
using Pharmacy.Event.Events;

namespace Notifications.Services;

public interface INotificationService
{
    Task SendToEmail(EmailMessage emailMessage, CancellationToken cancellationToken);

    Task ToPharmacyExpiryProduct(string email, DateTime day, int count, decimal totalPurchasePrice,
        decimal totalSalePrice, List<ExpiredItemEvent> expiryDateItems, CancellationToken cancellationToken);

    Task ToCeoReportCompletedOrders(string toEmail, DateTime day, int count, decimal totalAmount,
        CancellationToken cancellationToken);

    Task ToCustomerOrderCancelled(string toEmail, long orderId, DateTime cancelledAt,
        CancellationToken cancellationToken);

    Task ToCustomerOrderCompleted(string toEmail, long orderId, decimal totalAmount, DateTime completedAt,
        CancellationToken cancellationToken);

    Task ToCustomerOrderCreated(string toEmail, long orderId, decimal totalAmount, decimal deliveryFee,
        DateTime createdAt, CancellationToken cancellationToken);

    Task ToCustomerOrderShipped(string toEmail, long orderId, decimal deliveryFee, decimal totalAmount,
        DateTime shippedAt, string deliverName, CancellationToken cancellationToken);

    Task ToPharmacyLowStockProduct(string toEmail, string productName, int currentStock,
        CancellationToken cancellationToken);
}

public class NotificationService(
    IOptionsMonitor<EmailOptions> emailOption,
    ILogger<NotificationService> logger) : INotificationService
{
    public async Task SendToEmail(
        EmailMessage emailMessage,
        CancellationToken cancellationToken)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress(
            emailOption.CurrentValue.FromName ?? emailOption.CurrentValue.UserName
            ?? throw new InvalidOperationException("Email sender name (FromName) is not configured."),
            emailOption.CurrentValue.From
            ?? throw new InvalidOperationException("Email sender address (From) is not configured.")));

        email.To.Add(MailboxAddress.Parse(emailMessage.To));

        email.Subject = emailMessage.Subject;

        var builder = new BodyBuilder
        {
            HtmlBody = emailMessage.Body
        };

        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();

        smtp.CheckCertificateRevocation = false;

        await smtp.ConnectAsync(
            emailOption.CurrentValue.Host
            ?? throw new InvalidOperationException("SMTP host is not configured."),
            emailOption.CurrentValue.Port,
            SecureSocketOptions.StartTls,
            cancellationToken);

        await smtp.AuthenticateAsync(
            emailOption.CurrentValue.UserName
            ?? throw new InvalidOperationException("SMTP username is not configured."),
            emailOption.CurrentValue.Password
            ?? throw new InvalidOperationException("SMTP password is not configured."),
            cancellationToken);

        await smtp.SendAsync(
            email,
            cancellationToken);

        await smtp.DisconnectAsync(
            true,
            cancellationToken);

        logger.LogInformation(
            "Email sent to {Email} ",
            emailMessage.To);
    }


    public async Task ToPharmacyExpiryProduct(string email, DateTime day, int count, decimal totalPurchasePrice,
        decimal totalSalePrice,
        List<ExpiredItemEvent> expiryDateItems, CancellationToken cancellationToken)
    {
        var itemsHtml = string.Join(
            "",
            expiryDateItems.Select(item => $"""
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
            To = email,
            Subject = "Expired Products",
            Body = $"""
                    <html>
                    <body>
                        <h2>Expired Products Report</h2>
                        <p><strong>Date:</strong> {day}</p>
                        <p><strong>Total Count:</strong> {count}</p>
                        <p><strong>Total Purchase Price:</strong> {totalPurchasePrice:N2}</p>
                        <p><strong>Total Sale Price:</strong> {totalSalePrice:N2}</p>
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
        await SendToEmail(message, cancellationToken);
    }

    public async Task ToCeoReportCompletedOrders(string toEmail, DateTime day, int count, decimal totalAmount,
        CancellationToken cancellationToken)
    {
        var message = new EmailMessage()
        {
            To = toEmail,
            Subject = $"Report",
            Body = $@"
                <h2>Report Completed order</h2>
                <p>Day:<strong>{day}</strong></p>
                <p>Count:<strong>{count}</strong> </p>
                <p>TotalAmount:<strong>{totalAmount}</strong></p>"
        };
        await SendToEmail(message, cancellationToken);
    }

    public async Task ToCustomerOrderCancelled(string toEmail, long orderId, DateTime cancelledAt,
        CancellationToken cancellationToken)
    {
        var message = new EmailMessage
        {
            To = toEmail,
            Subject = $"Cancelled Order",
            Body = $@"
                <h2>Your order is cancelled</h2>
                <p>Order Id: <strong>#{orderId}</strong></p>
                <p>Order cancelled at <strong>{cancelledAt}</strong></p>
                
            "
        };
        await SendToEmail(message, cancellationToken);
    }

    public async Task ToCustomerOrderCompleted(string toEmail, long orderId, decimal totalAmount, DateTime completedAt,
        CancellationToken cancellationToken)
    {
        var message = new EmailMessage
        {
            To = toEmail,
            Subject = $"Order Completed",
            Body = $@"
                <h2>Your order is completed at<strong>{completedAt}</strong></h2>
                <p>Order id: <strong>#{orderId}</strong></p>
                <p>Total amount: <strong>{totalAmount:C}</strong></p>
                <p>Thanks for choose our  Pharmacy!</p>
            "
        };
        await SendToEmail(message, cancellationToken);
    }

    public async Task ToCustomerOrderCreated(string toEmail, long orderId, decimal totalAmount, decimal deliveryFee,
        DateTime createdAt,
        CancellationToken cancellationToken)
    {
        var message = new EmailMessage
        {
            To = toEmail,
            Subject = $"Order Created",
            Body = $@"
                <h2>Your order created</h2>
                <p>order Id: <strong>#{orderId}</strong></p>
                <p>DeliveryFee <strong>{deliveryFee:C}</strong></p>
                <p>Total amount <strong>{totalAmount:C}</strong></p>
        <p>Order created at <strong>{createdAt}</strong></p>
                <p>Thanks for  order in our Pharmacy</p>

            "
        };
        await SendToEmail(message, cancellationToken);
    }

    public async Task ToCustomerOrderShipped(string toEmail, long orderId, decimal deliveryFee, decimal totalAmount,
        DateTime shippedAt, string deliverName, CancellationToken cancellationToken)
    {
        var message = new EmailMessage
        {
            To = toEmail,
            Subject = $"Order Shipped",
            Body = $@"
                <h2>Your order shipped</h2>
                <p>order Id: <strong>#{orderId}</strong></p>
                <p>DeliveryFee <strong>{deliveryFee:C}</strong></p>
                <p>Total amount <strong>{totalAmount:C}</strong></p>
        <p>Order shipped at <strong>{shippedAt}</strong></p>
 <p>Deliver name is <strong>{deliverName}</strong></p>
                <p>Thanks for  order in our Pharmacy</p>

            "
        };
        await SendToEmail(message, cancellationToken);
    }

    public async Task ToPharmacyLowStockProduct(string toEmail, string productName, int currentStock,
        CancellationToken cancellationToken)
    {
        var message = new EmailMessage
        {
            To = toEmail,
            Subject = $"Low Stock",
            Body = $@"
                <h2>️ Low stock Product {productName}</h2>
                <p>Remaining: <strong>{currentStock} units.</strong></p>
                <p>Please replenish the stock</p>
            "
        };
        await SendToEmail(message, cancellationToken);
    }
}