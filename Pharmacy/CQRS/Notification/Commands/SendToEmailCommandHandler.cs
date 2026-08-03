using MailKit.Net.Smtp;
using MailKit.Security;
using MediatR;
using Microsoft.Extensions.Options;
using MimeKit;
using Pharmacy.Infrastructure.Setting;
using Pharmacy.Models.Domain;

namespace Pharmacy.CQRS.Notification.Commands;

public record SendToEmailCommand(
    EmailMessage Message) : IRequest;

public class SendToEmailCommandHandler(
    IOptionsMonitor<EmailOption> emailOption,
    ILogger<SendToEmailCommandHandler> logger) : IRequestHandler<SendToEmailCommand>
{
    public async Task Handle(SendToEmailCommand request, CancellationToken cancellationToken)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress(
            emailOption.CurrentValue.UserName,
            emailOption.CurrentValue.From));


        email.To.Add(MailboxAddress.Parse(request.Message.To));
        email.Subject = request.Message.Subject;

        var builder = new BodyBuilder { HtmlBody = request.Message.Body };
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(
                emailOption.CurrentValue.Host ?? throw new InvalidOperationException(),
                emailOption.CurrentValue.Port,
                SecureSocketOptions.StartTls, cancellationToken);
        }
        catch (System.Exception w)
        {
            Console.WriteLine(w);
            throw;
        }
        
        try
        {
            Console.WriteLine($"User: '{emailOption.CurrentValue.UserName}'");
            Console.WriteLine($"Password Length: {emailOption.CurrentValue.Password?.Length}");
            await smtp.AuthenticateAsync(
                emailOption.CurrentValue.UserName ?? throw new InvalidOperationException(),
                emailOption.CurrentValue.Password ?? throw new InvalidOperationException(),
                cancellationToken);
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "Failed to send email to {Email}", request.Message.To);
        }

        await smtp.SendAsync(email,
            cancellationToken);
        await smtp.DisconnectAsync(true,
            cancellationToken);

        logger.LogInformation("Email sent to {Email}", request.Message.To);
    }
}