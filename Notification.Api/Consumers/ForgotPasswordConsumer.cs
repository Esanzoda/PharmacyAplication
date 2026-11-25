using MassTransit;
using Notification.Api_.Application.Services;
using PharmacyService.Contract.Events;

namespace Notification.Api_.Consumers;

public class ForgotPasswordConsumer(
    IEmailService emailService) : IConsumer<ForgotPasswordEvent>
{
    public async Task Consume(ConsumeContext<ForgotPasswordEvent> context)
    {
        var message = context.Message;
        await emailService.ToUser(
            message.To,
            message.Message);
    }
}