using MassTransit;
using MediatR;
using Notifications.CQRS;
using Pharmacy.Event.Events;

namespace Notifications.Consumers;

public class ForgotPasswordConsumer(
    IMediator mediator) : IConsumer<ForgotPasswordEvent>
{
    public async Task Consume(ConsumeContext<ForgotPasswordEvent> context)
    {
        var message = context.Message;
        await mediator.Send(new ToUserCommand(
            message.To,
            message.Message));
    }
}