namespace SurveyBasket.Domain.Interfaces.Repositories;

public interface IRoleRepository
{
    // Queries
    Task<IEnumerable<ApplicationRole>> GetAllAsync(bool includeDisabled = false, CancellationToken ct = default);
    Task<ApplicationRole?> GetByIdAsync(string id, CancellationToken ct = default);
    Task<bool> ExistsByNameExceptIdAsync(string name, string? excludeId, CancellationToken ct = default);
    Task<IEnumerable<string>> GetPermissionsForRoleAsync(string roleId, CancellationToken ct = default);

    // Commands
    Task<Result> AddAsync(ApplicationRole role, IEnumerable<string> permissions, CancellationToken ct = default);
    Task<Result> UpdateAsync(ApplicationRole role, IEnumerable<string> permissions, CancellationToken ct = default);
    Task ToggleStatusAsync(string id, CancellationToken ct = default);

    // User Role Management
    Task<IList<string>> GetUserRolesAsync(string userId, CancellationToken ct = default);
    Task<IList<string>> GetDefaultRolesAsync(CancellationToken ct = default);
    Task<Result> ReplaceUserRolesAsync(string userId, IList<string> newRoles, CancellationToken ct = default);
    Task<Result> ResetUserToDefaultRoleAsync(string userId, CancellationToken ct = default);
}
