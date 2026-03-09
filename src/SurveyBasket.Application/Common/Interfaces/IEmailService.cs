namespace SurveyBasket.Application.Common.Interfaces;

public interface IEmailService
{
    /// <param name="toEmail">Recipient email address</param>
    /// <param name="displayName">Recipient's display name for greeting</param>
    /// <param name="confirmationLink">Full URL for email confirmation</param>
    Task SendConfirmationEmailAsync(
        string toEmail,
        string displayName,
        string confirmationLink,
        CancellationToken ct = default);

    /// <param name="toEmail">Recipient email address</param>
    /// <param name="displayName">Recipient's display name</param>
    /// <param name="resetLink">Full URL for password reset</param>
    Task SendPasswordResetEmailAsync(
        string toEmail,
        string displayName,
        string resetLink,
        CancellationToken ct = default);

    /// <param name="toEmail">Recipient email address</param>
    /// <param name="displayName">Recipient's display name</param>
    /// <param name="pollTitle">Title of the new poll</param>
    /// <param name="pollUrl">URL to access the poll</param>
    /// <param name="pollEndDate">When the poll closes</param>
    Task SendPollNotificationAsync(
        string toEmail,
        string displayName,
        string pollTitle,
        string pollUrl,
        DateOnly pollEndDate,
        CancellationToken ct = default);
}