using Microsoft.AspNetCore.Identity;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Errors;

namespace SurveyBasket.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository(
    UserManager<ApplicationUser> userManager) : IUserRepository
{
    // ── Queries ──────────────────────────────────────────────────────

    public async Task<ApplicationUser?> GetByIdAsync(string id, CancellationToken ct = default)
        => await userManager.FindByIdAsync(id);

    public async Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await userManager.FindByEmailAsync(email);

    public async Task<ApplicationUser?> GetByUsernameAsync(string username, CancellationToken ct = default)
        => await userManager.FindByNameAsync(username);

    public async Task<ApplicationUser?> GetByUserNameOrEmailAsync(string emailOrUserName, CancellationToken ct = default)
        => await userManager.FindByNameAsync(emailOrUserName)
            ?? await userManager.FindByEmailAsync(emailOrUserName);

    public async Task<bool> IsUsernameExistsAsync(string username, CancellationToken ct = default)
        => await userManager.Users.AnyAsync(u => u.UserName == username, ct);

    public async Task<bool> IsEmailExistsAsync(string email, CancellationToken ct = default)
        => await userManager.Users.AnyAsync(u => u.Email == email, ct);

    public async Task<bool> IsEmailConfirmedAsync(ApplicationUser user)
        => await userManager.IsEmailConfirmedAsync(user);

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        => await userManager.CheckPasswordAsync(user, password);

    // ── Commands — all wrap IdentityResult into Result ─────────────

    public async Task<Result> CreateAsync(ApplicationUser user, string password)
    {
        var identityResult = await userManager.CreateAsync(user, password);
        return ToResult(identityResult);
    }

    public async Task<Result> UpdateAsync(ApplicationUser user)
    {
        var identityResult = await userManager.UpdateAsync(user);
        return ToResult(identityResult);
    }

    public async Task<Result> ConfirmEmailAsync(ApplicationUser user, string token)
    {
        var identityResult = await userManager.ConfirmEmailAsync(user, token);
        return ToResult(identityResult);
    }

    // ── Token generation ──────────────────────────────────────────

    public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        => await userManager.GenerateEmailConfirmationTokenAsync(user);

    public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        => await userManager.GeneratePasswordResetTokenAsync(user);

    // ── Private helpers ───────────────────────────────────────────

    /// <summary>
    /// Converts IdentityResult to Result.
    /// All Identity error handling is contained here — never leaks to Application.
    /// </summary>
    private static Result ToResult(IdentityResult identityResult)
    {
        if (identityResult.Succeeded)
            return Result.Success();

        var error = identityResult.Errors.First();
        return Result.Failure(
            Error.Validation($"Identity.{error.Code}", error.Description));
    }
}