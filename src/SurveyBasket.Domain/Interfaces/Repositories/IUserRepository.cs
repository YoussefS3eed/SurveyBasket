using SurveyBasket.Domain.Common.Dtos;
using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    // ── Queries ──────────────────────────────────────────────────────
    Task<ApplicationUser?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<UserProfileDto> GetUserProfileByIdAsync(string id, CancellationToken ct = default);
    Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<ApplicationUser?> GetByUsernameAsync(string username, CancellationToken ct = default);
    Task<ApplicationUser?> GetByUserNameOrEmailAsync(string emailOrUserName, CancellationToken ct = default);

    Task<bool> IsUsernameExistsAsync(string username, CancellationToken ct = default);
    Task<bool> IsEmailExistsAsync(string email, CancellationToken ct = default);
    Task<bool> IsEmailConfirmedAsync(ApplicationUser user);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);

    // ── Commands — IdentityResult replaced with Result ────────────────
    Task<Result> CreateAsync(ApplicationUser user, string password);
    Task<Result> UpdateAsync(ApplicationUser user);
    Task<Result> ConfirmEmailAsync(ApplicationUser user, string token);

    // ── Token generation ──────────────────────────────────────────────
    Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
    Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);

    // ── Profile Management ─────────────────────────────────────────────
    Task UpdateProfileAsync(string userId, string firstname, string lastName);

    // ── Password Management ────────────────────────────────────────────
    Task<Result> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
    Task<Result> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);
}