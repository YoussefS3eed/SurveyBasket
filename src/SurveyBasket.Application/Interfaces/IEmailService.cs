using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Application.Interfaces;

public interface IEmailService
{
    Task SendConfirmationEmailAsync(ApplicationUser user, string code, CancellationToken cancellationToken = default);
    Task SendPasswordResetEmailAsync(ApplicationUser user, string code, CancellationToken cancellationToken = default);
}
