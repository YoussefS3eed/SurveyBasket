using Mapster;
using Microsoft.AspNetCore.Identity;
using SurveyBasket.Application.Common.Contracts;
using SurveyBasket.Domain.Common.Dtos;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Errors;

namespace SurveyBasket.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository(
    UserManager<ApplicationUser> userManager,
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

    public async Task<bool> IsEmailExistsAsync(string email, CancellationToken ct = default)
        => await userManager.Users.AnyAsync(u => u.Email == email, ct);

    public async Task<bool> IsEmailConfirmedAsync(ApplicationUser user)
        => await userManager.IsEmailConfirmedAsync(user);

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        => await userManager.CheckPasswordAsync(user, password);

    // Commands — all wrap IdentityResult into Result

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