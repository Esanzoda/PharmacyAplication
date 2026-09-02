using MassTransit;
using Notifications.Services;
using Pharmacy.Event.Events;

namespace Notifications.Consumers;

public class ForgotPasswordConsumer(
    INotificationService notificationService) : IConsumer<ForgotPasswordEvent>
{
    public async Task Consume(ConsumeContext<ForgotPasswordEvent> context)
    {
        var message = context.Message;
        await notificationService.ToUser(
            message.To,
            message.Message,
            context.CancellationToken);
    }
}