using Microsoft.AspNetCore.Identity;
using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Domain.Interfaces;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByIdAsync(string id);
    Task<ApplicationUser?> GetByUsernameAsync(string username);
    Task<ApplicationUser?> GetByEmailAsync(string email);
    Task<ApplicationUser?> GetByUserNameOrEmailAsync(string emailOrUserName);
    Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
    Task<IdentityResult> UpdateAsync(ApplicationUser user);
    Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token);
    Task AddRefreshTokenAsync(ApplicationUser user, RefreshToken refreshToken);
    Task RevokeRefreshTokenAsync(ApplicationUser user, string refreshToken);
    Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
    Task<bool> IsUsernameExistsAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> IsEmailConfirmedAsync(ApplicationUser user);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
}
