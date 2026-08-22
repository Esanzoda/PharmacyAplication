using MassTransit;
using Notifications.Services;
using Pharmacy.Event.Events;

namespace Notifications.Consumers;

public class CheckExpiryDateProductConsumer(
    INotificationService notificationService) : IConsumer<CheckExpiredProductEvent>
{
    public async Task Consume(ConsumeContext<CheckExpiredProductEvent> context)
    {
        var message = context.Message;
        await notificationService.ToPharmacyExpiryProduct(
            message.To,
            message.Day,
            message.Count,
            message.TotalPurchasePrice,
            message.TotalSalePrice,
            message.ExpiryDateItems,
            context.CancellationToken);
    }
}