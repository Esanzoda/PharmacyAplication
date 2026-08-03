using Pharmacy.Models.Domain;

namespace Pharmacy.Services.Message;

public interface IMessageService
{
    Task<string> SendOrderCreatedAsync(string toPhoneNumber, long orderId, decimal totalAmount, DateTime createdAt);
    Task<string> SendOrderCancelledAsync(string toPhoneNumber, long orderId, DateTime updatedAt);
    Task<string> SendOrderCompletedAsync(string toPhoneNumber, long orderId, decimal totalAmount, DateTime completedAt);
    Task<string> SendLowStockAlertAsync(string toPhoneNumber, string productName, int currentStock);
}

public class MessageService : IMessageService
{
    public Task<string> SendOrderCreatedAsync(string toPhonenumber, long orderId, decimal totalAmount,
        DateTime createdAt)
    {
        var messageText = $"Order created at {createdAt} total amount {totalAmount}";
        var message = new PhoneNumberMessage
        {
            Message = messageText,
        };

        return Task.FromResult(message.Message);
    }

    public Task<string> SendOrderCancelledAsync(string toPhonenumber, long orderId, DateTime updatedAt)
    {
        var messageText = $"Order canceled at {updatedAt}";
        var message = new PhoneNumberMessage
        {
            Message = messageText,
        };

        return Task.FromResult(message.Message);
    }

    public Task<string> SendOrderCompletedAsync(string toPhonenumber, long orderId, decimal totalAmount,
        DateTime completedAt)
    {
        var messageText = $"Order completed at {completedAt} total amount {totalAmount} send to {toPhonenumber}";
        Console.WriteLine(messageText);
        var message = new PhoneNumberMessage
        {
            Message = messageText,
        };

        return Task.FromResult(message.Message);
    }

    public Task<string> SendLowStockAlertAsync(string toPhonenumber, string productName, int currentStock)
    {
        var messageText = $"Low stock product {productName} quantity {currentStock} send to {toPhonenumber}";
        Console.WriteLine((messageText));
        var message = new PhoneNumberMessage
        {
            Message = messageText,
        };
        return Task.FromResult(message.Message);
    }
}