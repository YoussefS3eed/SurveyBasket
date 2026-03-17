using Microsoft.AspNetCore.Identity;
using SurveyBasket.Application.Common.Contracts;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Errors;

namespace SurveyBasket.Infrastructure.Persistence.Repositories;

internal sealed class RoleRepository(
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext context) : IRoleRepository
{
    public async Task<IEnumerable<ApplicationRole>> GetAllAsync(bool includeDisabled, CancellationToken ct = default)
    {
        return await roleManager.Roles
            .Where(x => !x.IsDefault && (!x.IsDeleted || includeDisabled))
            .ToListAsync(ct);
    }

    public async Task<ApplicationRole?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        return await roleManager.FindByIdAsync(id);
    }

    public async Task<bool> ExistsByNameExceptIdAsync(string name, string? excludeId, CancellationToken ct = default)
    {
        return await roleManager.Roles
            .AnyAsync(x => x.Name == name && (excludeId == null || x.Id != excludeId), ct);
    }

    public async Task<IEnumerable<string>> GetPermissionsForRoleAsync(string roleId, CancellationToken ct = default)
    {
        return await context.RoleClaims
            .Where(x => x.RoleId == roleId && x.ClaimType == Permissions.Type)
            .Select(x => x.ClaimValue!)
            .ToListAsync(ct);
    }

    public async Task<Result> AddAsync(ApplicationRole role, IEnumerable<string> permissions, CancellationToken ct = default)
    {
        var result = await roleManager.CreateAsync(role);

        if (!result.Succeeded)
            return ToResult(result);

        var roleClaims = permissions
            .Select(x => new IdentityRoleClaim<string>
            {
                ClaimType = Permissions.Type,
                ClaimValue = x,
                RoleId = role.Id
            });

        await context.AddRangeAsync(roleClaims, ct);
        await context.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> UpdateAsync(ApplicationRole role, IEnumerable<string> permissions, CancellationToken ct = default)
    {
        var identityResult = await roleManager.UpdateAsync(role);

        if (!identityResult.Succeeded)
            return ToResult(identityResult);

        var currentPermissions = await context.RoleClaims
            .Where(x => x.RoleId == role.Id && x.ClaimType == Permissions.Type)
            .Select(x => x.ClaimValue)
            .ToListAsync(ct);

        var newPermissions = permissions.Except(currentPermissions)
            .Select(x => new IdentityRoleClaim<string>
            {
                ClaimType = Permissions.Type,
                ClaimValue = x,
                RoleId = role.Id
            });

        var removedPermissions = currentPermissions.Except(permissions);

        await context.RoleClaims
            .Where(x => x.RoleId == role.Id && removedPermissions.Contains(x.ClaimValue))
            .ExecuteDeleteAsync(ct);

        await context.AddRangeAsync(newPermissions, ct);
        await context.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task ToggleStatusAsync(string id, CancellationToken ct = default)
    {
        var role = await roleManager.FindByIdAsync(id);
        if (role is not null)
        {
            role.IsDeleted = !role.IsDeleted;
            await roleManager.UpdateAsync(role);
        }
    }

    // User Role Management
    public async Task<IList<string>> GetUserRolesAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Array.Empty<string>();

        return await userManager.GetRolesAsync(user);
    }

    public async Task<IList<string>> GetDefaultRolesAsync(CancellationToken ct = default)
    {
        return await roleManager.Roles
            .Where(r => r.IsDefault && !r.IsDeleted)
            .Select(r => r.Name!)
            .ToListAsync(ct);
    }

    public async Task<Result> ReplaceUserRolesAsync(string userId, IList<string> newRoles, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.NotFound(userId));

        // Get current roles and default roles
        var currentRoles = await userManager.GetRolesAsync(user);
        var defaultRoles = await GetDefaultRolesAsync(ct);

        // Always keep default roles
        var rolesToKeep = currentRoles.Intersect(defaultRoles).ToList();

        // Determine roles to add and remove (excluding default roles)
        var rolesToAdd = newRoles.Except(currentRoles).Except(defaultRoles).ToList();
        var rolesToRemove = currentRoles.Where(r => !newRoles.Contains(r) && !defaultRoles.Contains(r)).ToList();

        // Remove extra roles (non-default only)
        if (rolesToRemove.Any())
        {
            var removeResult = await userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
                return ToResult(removeResult);
        }

        // Add new roles (non-default only)
        if (rolesToAdd.Any())
        {
            var addResult = await userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addResult.Succeeded)
                return ToResult(addResult);
        }

        return Result.Success();
    }

    public async Task<Result> ResetUserToDefaultRoleAsync(string userId, CancellationToken ct = default)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure(UserErrors.NotFound(userId));

        // Get default roles and current roles
        var defaultRoles = await GetDefaultRolesAsync(ct);
        var currentRoles = await userManager.GetRolesAsync(user);

        // Remove all non-default roles
        var rolesToRemove = currentRoles.Where(r => !defaultRoles.Contains(r)).ToList();

        if (rolesToRemove.Any())
        {
            var removeResult = await userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
                return ToResult(removeResult);
        }

        // Ensure user has all default roles
        var rolesToAdd = defaultRoles.Except(currentRoles).ToList();
        if (rolesToAdd.Any())
        {
            var addResult = await userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addResult.Succeeded)
                return ToResult(addResult);
        }

        return Result.Success();
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
