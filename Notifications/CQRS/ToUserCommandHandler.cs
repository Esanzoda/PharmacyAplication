using MediatR;
using Notifications.Models;

namespace Notifications.CQRS;

public record ToUserCommand(
    string ToEmail,
    string Message) : IRequest;

public class ToUserCommandHandler(
    IMediator mediator) : IRequestHandler<ToUserCommand>
{
    public async Task Handle(ToUserCommand request, CancellationToken cancellationToken)
    {
        var message = new EmailMessage
        {
            To = request.ToEmail,
            Subject = "Information",
            Body = request.Message
        };
        await mediator.Send(new SendToEmailCommand(message), cancellationToken);
    }
}