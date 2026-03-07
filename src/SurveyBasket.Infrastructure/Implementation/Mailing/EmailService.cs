using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SurveyBasket.Application.Interfaces;
using SurveyBasket.Infrastructure.Helpers;
using SurveyBasket.Infrastructure.Settings;


namespace SurveyBasket.Infrastructure.Implementation.Mailing;

public class EmailService(
    IOptions<MailSettings> mailSettings,
    IHttpContextAccessor httpContextAccessor,
    ILogger<EmailService> logger) : IEmailService
{
    private readonly MailSettings _mailSettings = mailSettings.Value;

    public async Task SendConfirmationEmailAsync(ApplicationUser user, string code, CancellationToken cancellationToken = default)
    {
        var origin = httpContextAccessor.HttpContext?.Request.Headers.Origin;

        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            new Dictionary<string, string>
            {
                { "{{name}}", $"{user.FirstName} {user.LastName}" },
                { "{{action_url}}", $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}" }
            }
        );
        //logger.LogError("The {Orgin} is: ", origin);

        await SendEmailAsync(user.Email!, "✅ Survey Basket: Email Confirmation", emailBody, cancellationToken);
    }

    public async Task SendPasswordResetEmailAsync(ApplicationUser user, string code, CancellationToken cancellationToken = default)
    {
        var origin = httpContextAccessor.HttpContext?.Request.Headers.Origin.ToString();

        var emailBody = EmailBodyBuilder.GenerateEmailBody("PasswordReset",
            new Dictionary<string, string>
            {
                { "{{name}}", $"{user.FirstName} {user.LastName}" },
                { "{{action_url}}", $"{origin}/auth/resetPassword?email={user.Email}&code={code}" }
            }
        );

        await SendEmailAsync(user.Email!, "🔐 Survey Basket: Password Reset", emailBody, cancellationToken);
    }

    private async Task SendEmailAsync(string email, string subject, string htmlMessage, CancellationToken cancellationToken = default)
    {
        var message = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_mailSettings.Mail),
            Subject = subject
        };

        message.To.Add(MailboxAddress.Parse(email));

        var builder = new BodyBuilder
        {
            HtmlBody = htmlMessage
        };

        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();

        logger.LogInformation("Sending email to {Email}", email);

        await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls, cancellationToken);
        await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password, cancellationToken);
        await smtp.SendAsync(message, cancellationToken);
        await smtp.DisconnectAsync(true, cancellationToken);
    }
}
