using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using SurveyBasket.Infrastructure.Services.Email;

namespace SurveyBasket.Infrastructure.Health;

public class MailProviderHealthCheck(IOptions<EmailSettings> emailSettings) : IHealthCheck
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var smtp = new SmtpClient();

            smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls, cancellationToken);
            smtp.Authenticate(_emailSettings.Mail, _emailSettings.Password, cancellationToken);

            return await Task.FromResult(HealthCheckResult.Healthy());
        }
        catch (Exception exception)
        {
            return await Task.FromResult(HealthCheckResult.Unhealthy(exception: exception));
        }
    }
}
