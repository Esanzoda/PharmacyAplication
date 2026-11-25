using MassTransit;
using Notification.Api_.Application.Services;
using PharmacyService.Contract.Events;

namespace Notification.Api_.Consumers;

public class CheckExpiryDateProductConsumer(
    IEmailService emailService) : IConsumer<CheckExpiredProductEvent>
{
    public async Task Consume(ConsumeContext<CheckExpiredProductEvent> context)
    {
        var message = context.Message;

        await emailService.ToPharmacyExpiryProduct(
            message.To,
            message.Day,
            message.Count,
            message.TotalPurchasePrice,
            message.TotalSalePrice,
            message.ExpiryDateItems);
    }
}