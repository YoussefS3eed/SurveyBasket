using Mapster;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using SurveyBasket.Application.Common.Contracts;
using SurveyBasket.Domain.Common.Dtos;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Errors;

namespace SurveyBasket.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ApplicationDbContext context) : IUserRepository
{
    // Queries

    public async Task<ApplicationUser?> GetByIdAsync(string id, CancellationToken ct = default)
        => await userManager.FindByIdAsync(id);

    public async Task<UserProfileDto> GetUserProfileByIdAsync(string id, CancellationToken ct = default)
        => await userManager.Users
            .Where(x => x.Id == id)
            .ProjectToType<UserProfileDto>()
            .FirstAsync(ct);

    public async Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await userManager.FindByEmailAsync(email);

    public async Task<ApplicationUser?> GetByUsernameAsync(string username, CancellationToken ct = default)
        => await userManager.FindByNameAsync(username);

    public async Task<ApplicationUser?> GetByUserNameOrEmailAsync(string emailOrUserName, CancellationToken ct = default)
        => await userManager.FindByNameAsync(emailOrUserName)
            ?? await userManager.FindByEmailAsync(emailOrUserName);

    public async Task<bool> IsUsernameExistsAsync(string username, CancellationToken ct = default)
        => await userManager.Users.AnyAsync(u => u.UserName == username, ct);

    public async Task<bool> IsUsernameExistsAsync(string username, string excludeId, CancellationToken ct = default)
        => await userManager.Users.AnyAsync(u => u.UserName == username && u.Id != excludeId, ct);

    public async Task<bool> IsEmailExistsAsync(string email, CancellationToken ct = default)
        => await userManager.Users.AnyAsync(u => u.Email == email, ct);

    public async Task<bool> IsEmailExistsAsync(string email, string id, CancellationToken ct = default)
        => await userManager.Users.AnyAsync(u => u.Email == email && u.Id != id, ct);


    public async Task<bool> IsEmailConfirmedAsync(ApplicationUser user)
        => await userManager.IsEmailConfirmedAsync(user);

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password, bool isPersistent, bool lokkoutOnFailure)
        => (await signInManager.PasswordSignInAsync(user, password, isPersistent, lokkoutOnFailure)).Succeeded;

    // User Management Queries

    public async Task<IEnumerable<(ApplicationUser user, List<string> roles)>> GetAllWithRolesAsync(CancellationToken ct = default)
    {
        var users = await (from u in context.Users
                           join ur in context.UserRoles
                           on u.Id equals ur.UserId
                           join r in context.Roles
                           on ur.RoleId equals r.Id into roles
                           where !roles.Any(x => x.Name == DefaultRoles.Member)
                           select new
                           {
                               u,
                               Roles = roles.Select(x => x.Name!).ToList()
                           })
                            .GroupBy(x => x.u.Id)
                            .Select(g => new
                            {
                                User = g.First().u,
                                Roles = g.SelectMany(x => x.Roles).ToList()
                            })
                            .ToListAsync(ct);

        return users.Select(x => (x.User, x.Roles));
    }

    public async Task<IEnumerable<string>> GetRolesAsync(ApplicationUser user)
        => [.. (await userManager.GetRolesAsync(user))];

    // Commands — all wrap IdentityResult into Result
    public async Task<Result> CreateAsync(ApplicationUser user)
    {
        var identityResult = await userManager.CreateAsync(user);
        return ToResult(identityResult);
    }
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

    // Token generation

    public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        => await userManager.GenerateEmailConfirmationTokenAsync(user);

    public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        => await userManager.GeneratePasswordResetTokenAsync(user);

    // Profile Management

    public async Task UpdateProfileAsync(string userId, string firstname, string lastName)
    {
        await userManager.Users.Where(x => x.Id == userId)
            .ExecuteUpdateAsync(setters =>
                setters
                    .SetProperty(x => x.FirstName, firstname)
                    .SetProperty(x => x.LastName, lastName)
            );
    }

    public async Task<Result> UpdateEmailAsync(string userId, string newEmail, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.NotFound(userId));

        var token = await userManager.GenerateChangeEmailTokenAsync(user, newEmail);
        var identityResult = await userManager.ChangeEmailAsync(user, newEmail, token);
        return ToResult(identityResult);
    }

    // Password Management

    public async Task<Result> ChangePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
    {
        var identityResult = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        return ToResult(identityResult);
    }

    public async Task<Result> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
    {
        var identityResult = await userManager.ResetPasswordAsync(user, token, newPassword);
        return ToResult(identityResult);
    }

    // Roles and Permissions

    public async Task<(IEnumerable<string> roles, IEnumerable<string> permissions)> GetUserRolesAndPermissionsAsync(ApplicationUser user, CancellationToken ct = default)
    {
        var userRoles = await userManager.GetRolesAsync(user);

        var userPermissions = await (
            from r in context.Roles
            join rc in context.RoleClaims on r.Id equals rc.RoleId
            where userRoles.Contains(r.Name!) && rc.ClaimType == Permissions.Type
            select rc.ClaimValue!)
            .Distinct()
            .ToListAsync(ct);
        return (userRoles, userPermissions);
    }

    public async Task AddToRoleAsync(ApplicationUser user, string roleName) =>
        await userManager.AddToRoleAsync(user, roleName);

    public async Task<Result> AddToRolesAsync(string userId, IEnumerable<string> roles, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.NotFound(userId));

        var currentRoles = await userManager.GetRolesAsync(user);
        var rolesToAdd = roles.Except(currentRoles).ToList();

        if (rolesToAdd.Count == 0)
            return Result.Success();

        var identityResult = await userManager.AddToRolesAsync(user, rolesToAdd);
        return ToResult(identityResult);
    }

    public async Task<Result> ToggleStatusAsync(string id, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
            return Result.Failure(UserErrors.NotFound(id));

        user.IsDisabled = !user.IsDisabled;
        var identityResult = await userManager.UpdateAsync(user);
        return ToResult(identityResult);
    }

    public async Task<Result> UnlockAsync(string id, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(id);
        if (user is null)
            return Result.Failure(UserErrors.NotFound(id));

        user.LockoutEnd = null;
        var identityResult = await userManager.UpdateAsync(user);
        return ToResult(identityResult);
    }

    public async Task SetEmailVerificationCodeAsync(string userId,  string newEmail, string code, DateTime expiresAt, CancellationToken ct = default)
    {
        // Invalidate any existing codes for this user
        var existingCodes = context.EmailVerificationCodes
            .Where(evc => evc.UserId == userId && !evc.IsUsed && evc.ExpiresAt > DateTime.UtcNow);
        
        if (existingCodes.Any())
        {
            foreach (var existingCode in existingCodes)
            {
                existingCode.IsUsed = true;
            }
        }

        // Create new verification code with the new email
        var verificationCode = new EmailVerificationCode
        {
            UserId = userId,
            Code = code,
            NewEmail = newEmail,
            ExpiresAt = expiresAt
        };

        await context.EmailVerificationCodes.AddAsync(verificationCode, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task<(bool IsValid, string? NewEmail)> VerifyEmailCodeAsync(string userId, string code, CancellationToken ct = default)
    {
        var verificationCode = await context.EmailVerificationCodes
            .FirstOrDefaultAsync(evc =>
                evc.UserId == userId &&
                evc.Code == code &&
                !evc.IsUsed &&
                evc.ExpiresAt > DateTime.UtcNow, ct);

        if (verificationCode is null)
            return (false, null);

        // Mark code as used
        verificationCode.IsUsed = true;
        await context.SaveChangesAsync(ct);

        return (true, verificationCode.NewEmail);
    }

    public async Task<(bool IsValid, string? NewEmail, string? UserId)> VerifyEmailCodeAsync(string code, CancellationToken ct = default)
    {
        var verificationCode = await context.EmailVerificationCodes
            .FirstOrDefaultAsync(evc =>
                evc.Code == code &&
                !evc.IsUsed &&
                evc.ExpiresAt > DateTime.UtcNow, ct);

        if (verificationCode is null)
            return (false, null, null);

        // Mark code as used
        verificationCode.IsUsed = true;
        await context.SaveChangesAsync(ct);

        return (true, verificationCode.NewEmail, verificationCode.UserId);
    }

    public async Task<EmailVerificationCodeInfo?> GetPendingEmailVerificationAsync(string userId, string newEmail, CancellationToken ct = default)
    {
        var verificationCode = await context.EmailVerificationCodes
            .FirstOrDefaultAsync(evc =>
                evc.UserId == userId &&
                evc.NewEmail == newEmail &&
                !evc.IsUsed &&
                evc.ExpiresAt > DateTime.UtcNow, ct);

        if (verificationCode is null)
            return null;

        return new EmailVerificationCodeInfo(
            verificationCode.Code,
            verificationCode.NewEmail,
            verificationCode.ExpiresAt,
            verificationCode.IsUsed);
    }

    // Private helpers
    private static Result ToResult(IdentityResult identityResult)
    {
        if (identityResult.Succeeded)
            return Result.Success();

        var errors = identityResult.Errors
            .Select(e => new ValidationError(e.Code, e.Description))
            .ToList();

        return Result.Failure(Error.Validation(errors));
    }
}