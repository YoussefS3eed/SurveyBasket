namespace SurveyBasket.Application.Common.Interfaces;

public interface IEmailService
{
    Task SendConfirmationEmailAsync(string toEmail, string displayName, string confirmationLink, CancellationToken ct = default);
    Task SendPasswordResetEmailAsync(string toEmail, string displayName, string resetLink, CancellationToken ct = default);
    Task SendPollNotificationAsync(string toEmail, string displayName, string pollTitle, string pollUrl, DateOnly pollEndDate, CancellationToken ct = default);
    Task SendEmailVerificationCodeAsync(string toEmail, string displayName, string verificationCode, CancellationToken ct = default);
    Task SendUserCreatedEmailAsync(string toEmail, string displayName, string username, string password, string confirmationLink, CancellationToken ct = default);
    Task SendAdminEmailChangeConfirmationAsync(string toEmail, string displayName, string username, string temporaryPassword, string confirmEmailLink, CancellationToken ct = default);
    Task SendEmailConfirmationRequestAsync(string toEmail, string displayName, string confirmEmailLink, CancellationToken ct = default);
}