using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Domain.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
}
