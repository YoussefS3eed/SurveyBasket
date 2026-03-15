using Microsoft.AspNetCore.Identity;
using SurveyBasket.Application.Common.Contracts;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Errors;

namespace SurveyBasket.Infrastructure.Persistence.Repositories;

internal sealed class RoleRepository(
    RoleManager<ApplicationRole> roleManager,
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
