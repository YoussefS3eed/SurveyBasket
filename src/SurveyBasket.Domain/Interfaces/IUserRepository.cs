using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Domain.Interfaces;

public interface IUserRepository
{
    Task UpdateUserAsync(ApplicationUser user);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<ApplicationUser?> GetByEmailAsync(string email);
}
