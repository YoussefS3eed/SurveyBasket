using SurveyBasket.Domain.Common.Dtos;

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
    Task<bool> IsUsernameExistsAsync(string username, string excludeId, CancellationToken ct = default);
    Task<bool> IsEmailExistsAsync(string email, CancellationToken ct = default);
    Task<bool> IsEmailExistsAsync(string email, string id, CancellationToken ct = default);
    Task<bool> IsEmailConfirmedAsync(ApplicationUser user);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password, bool isPersistent, bool lokkoutOnFailure);

    // User Management Queries
    Task<IEnumerable<(ApplicationUser user, List<string> roles)>> GetAllWithRolesAsync(CancellationToken ct = default);
    Task<IEnumerable<string>> GetRolesAsync(ApplicationUser user);

    // ── Commands — IdentityResult replaced with Result ────────────────
    Task<Result> CreateAsync(ApplicationUser user);
    Task<Result> CreateAsync(ApplicationUser user, string password);
    Task<Result> UpdateAsync(ApplicationUser user);
    Task<Result> ConfirmEmailAsync(ApplicationUser user, string token);

    // ── Token generation ──────────────────────────────────────────────
    Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
    Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);

    // ── Profile Management ─────────────────────────────────────────────
    Task UpdateProfileAsync(string userId, string firstname, string lastName);
    Task<Result> UpdateEmailAsync(string userId, string newEmail, CancellationToken ct = default);

    // ── Password Management ────────────────────────────────────────────
    Task<Result> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
    Task<Result> ResetPasswordAsync(ApplicationUser user, string token, string newPassword);

    // ── Roles and Permissions ─────────────────────────────────────────
    Task<(IEnumerable<string> roles, IEnumerable<string> permissions)> GetUserRolesAndPermissionsAsync(ApplicationUser user, CancellationToken ct = default);
    Task AddToRoleAsync(ApplicationUser user, string roleName);
    Task<Result> AddToRolesAsync(string userId, IEnumerable<string> roles, CancellationToken ct = default);

    // ── User Status Management ────────────────────────────────────────
    Task<Result> ToggleStatusAsync(string id, CancellationToken ct = default);
    Task<Result> UnlockAsync(string id, CancellationToken ct = default);

    // ── Email Verification Code ───────────────────────────────────────
    Task SetEmailVerificationCodeAsync(string userId, string newEmail, string code, DateTime expiresAt, CancellationToken ct = default);
    Task<(bool IsValid, string? NewEmail)> VerifyEmailCodeAsync(string userId, string code, CancellationToken ct = default);
    Task<(bool IsValid, string? NewEmail, string? UserId)> VerifyEmailCodeAsync(string code, CancellationToken ct = default);
    Task<EmailVerificationCodeInfo?> GetPendingEmailVerificationAsync(string userId, string newEmail, CancellationToken ct = default);
}