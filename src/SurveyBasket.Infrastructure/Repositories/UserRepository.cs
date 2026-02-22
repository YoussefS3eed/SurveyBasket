using Microsoft.AspNetCore.Identity;
using SurveyBasket.Domain.Entities;
using SurveyBasket.Domain.Interfaces;

namespace SurveyBasket.Infrastructure.Repositories;

public class UserRepository(UserManager<ApplicationUser> userManager) : IUserRepository
{
    public async Task UpdateUserAsync(ApplicationUser user)
        => await userManager.UpdateAsync(user);

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    => await userManager.CheckPasswordAsync(user, password);

    public async Task<ApplicationUser?> GetByIdAsync(string id)
        => await userManager.FindByIdAsync(id);

    public async Task<ApplicationUser?> GetByEmailAsync(string email)
        => await userManager.FindByEmailAsync(email);
}