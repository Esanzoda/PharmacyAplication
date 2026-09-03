using MailKit.Security;
using MediatR;
using MimeKit;
using Notifications.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using Notifications.Setting;

namespace Notifications.CQRS;

public record SendToEmailCommand(
    EmailMessage EmailMessage) : IRequest;

public class SendToEmailCommandHandler(
    IOptionsMonitor<EmailOptions> emailOption,
    ILogger<SendToEmailCommandHandler> logger) : IRequestHandler<SendToEmailCommand>
{
    public async Task Handle(SendToEmailCommand request, CancellationToken cancellationToken)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress(
            emailOption.CurrentValue.FromName,
            emailOption.CurrentValue.From
            ?? throw new InvalidOperationException("Email sender address (From) is not configured.")));

        email.To.Add(MailboxAddress.Parse(request.EmailMessage.To));

        email.Subject = request.EmailMessage.Subject;

        var builder = new BodyBuilder
        {
            HtmlBody = request.EmailMessage.Body
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
            request.EmailMessage.To);
    }
}