using Microsoft.AspNetCore.Identity;
using SurveyBasket.Domain.Entities;
using SurveyBasket.Domain.Interfaces;

namespace SurveyBasket.Infrastructure.Repositories;

public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
{
    public async Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        => await userManager.FindByEmailAsync(email);

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        => await userManager.CheckPasswordAsync(user, password);
}