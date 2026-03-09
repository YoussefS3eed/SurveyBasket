using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SurveyBasket.Application.Common.Interfaces;


namespace SurveyBasket.Infrastructure.Services.Email;

internal sealed class EmailService(
    IOptions<EmailSettings> mailSettings,
    ILogger<EmailService> logger) : IEmailService
{
    private readonly EmailSettings _settings = mailSettings.Value;

    // ✅ Pure primitives — no ApplicationUser
    public async Task SendConfirmationEmailAsync(
        string toEmail,
        string displayName,
        string confirmationLink,
        CancellationToken ct = default)
    {
        var body = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            new Dictionary<string, string>
            {
                { "{{name}}",       displayName       },
                { "{{action_url}}", confirmationLink  }
            });

        await SendAsync(toEmail, "✅ Survey Basket: Email Confirmation", body, ct);
    }

    public async Task SendPasswordResetEmailAsync(
        string toEmail,
        string displayName,
        string resetLink,
        CancellationToken ct = default)
    {
        var body = EmailBodyBuilder.GenerateEmailBody("PasswordReset",
            new Dictionary<string, string>
            {
                { "{{name}}",       displayName },
                { "{{action_url}}", resetLink   }
            });

        await SendAsync(toEmail, "🔐 Survey Basket: Password Reset", body, ct);
    }

    public async Task SendPollNotificationAsync(
        string toEmail,
        string displayName,
        string pollTitle,
        string pollUrl,
        DateOnly pollEndDate,
        CancellationToken ct = default)
    {
        var body = EmailBodyBuilder.GenerateEmailBody("PollNotification",
            new Dictionary<string, string>
            {
                { "{{name}}",    displayName              },
                { "{{pollTill}}", pollTitle               },
                { "{{endDate}}", pollEndDate.ToString()   },
                { "{{url}}",     pollUrl                  }
            });

        await SendAsync(toEmail, $"📣 Survey Basket: New Poll - {pollTitle}", body, ct);
    }

    // ── Private ───────────────────────────────────────────────────────

    private async Task SendAsync(
        string toEmail,
        string subject,
        string htmlBody,
        CancellationToken ct = default)
    {
        var message = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_settings.Mail),
            Subject = subject,
            Body = new BodyBuilder { HtmlBody = htmlBody }.ToMessageBody()
        };

        message.To.Add(MailboxAddress.Parse(toEmail));

        using var smtp = new SmtpClient();

        logger.LogInformation("Sending email to {Email} — Subject: {Subject}", toEmail, subject);

        await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
        await smtp.AuthenticateAsync(_settings.Mail, _settings.Password, ct);
        await smtp.SendAsync(message, ct);
        await smtp.DisconnectAsync(true, ct);
    }
}
