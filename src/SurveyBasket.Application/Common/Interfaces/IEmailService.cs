namespace SurveyBasket.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendConfirmationEmailAsync(string toEmail, string displayName, string confirmationLink, CancellationToken ct = default);
    Task SendPasswordResetEmailAsync(string toEmail, string displayName, string resetLink, CancellationToken ct = default);
    Task SendPollNotificationAsync(string toEmail, string displayName, string pollTitle, string pollUrl, DateOnly pollEndDate, CancellationToken ct = default);
}