using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.Infrastructure.Repositories;

public class UserRepository(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager) : IUserRepository
{
    public async Task<ApplicationUser?> GetByIdAsync(string id)
        => await userManager.FindByIdAsync(id);

    public async Task<ApplicationUser?> GetByUsernameAsync(string username)
        => await userManager.FindByNameAsync(username);

    public async Task<ApplicationUser?> GetByEmailAsync(string email)
        => await userManager.FindByEmailAsync(email);

    public async Task<ApplicationUser?> GetByUserNameOrEmailAsync(string emailOrUserName)
        => await userManager.FindByNameAsync(emailOrUserName)
            ?? await userManager.FindByEmailAsync(emailOrUserName);

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        => await userManager.CreateAsync(user, password);

    public async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        => await userManager.UpdateAsync(user);

    public async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
    => await userManager.ConfirmEmailAsync(user, token);

    public async Task AddRefreshTokenAsync(ApplicationUser user, RefreshToken refreshToken)
    {
        user.RefreshTokens.Add(refreshToken);
        await userManager.UpdateAsync(user);
    }

    public async Task RevokeRefreshTokenAsync(ApplicationUser user, string refreshToken)
    {
        var token = user.RefreshTokens.FirstOrDefault(rt => rt.Token == refreshToken && rt.IsActive);
        if (token is not null)
        {
            token.RevokedOn = DateTime.UtcNow;
            await userManager.UpdateAsync(user);
        }
    }

    public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        => await userManager.GenerateEmailConfirmationTokenAsync(user);

    public async Task<bool> IsUsernameExistsAsync(string username, CancellationToken cancellationToken = default)
        => await userManager.Users.AnyAsync(u => u.UserName == username, cancellationToken);

    public async Task<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken = default)
        => await userManager.Users.AnyAsync(u => u.Email == email, cancellationToken);

    public async Task<bool> IsEmailConfirmedAsync(ApplicationUser user)
        => await userManager.IsEmailConfirmedAsync(user);

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    => await userManager.CheckPasswordAsync(user, password);

    public async Task<SignInResult> SignInAsync(string userName, string password, bool isPersistent = false, bool lockOnFailure = false)
        => await signInManager.PasswordSignInAsync(userName, password, isPersistent, lockOnFailure);
}