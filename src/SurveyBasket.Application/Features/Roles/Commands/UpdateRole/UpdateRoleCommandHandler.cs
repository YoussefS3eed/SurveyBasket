using SurveyBasket.Application.Common.Contracts;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Roles.Commands.UpdateRole;

public class UpdateRoleCommandHandler(IRoleRepository roleRepository)
    : IRequestHandler<UpdateRoleCommand, Result>
{
    public async Task<Result> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        // Check if role exists
        var role = await roleRepository.GetByIdAsync(request.Id, cancellationToken);
        if (role is null)
            return Result.Failure(RoleErrors.RoleNotFound(request.Id));

        // Check for duplicate name
        var exists = await roleRepository.ExistsByNameExceptIdAsync(request.Name, request.Id, cancellationToken);
        if (exists)
            return Result.Failure(RoleErrors.DuplicateRole(request.Name));

        // Validate permissions
        var allowedPermissions = Permissions.GetAllPermissions();
        if (request.Permissions.Except(allowedPermissions).Any())
            return Result.Failure(RoleErrors.InvalidPermissions);

        // Update role
        role.Name = request.Name;
        var updateResult = await roleRepository.UpdateAsync(role, request.Permissions, cancellationToken);
        if (updateResult.IsFailure)
            return updateResult;

        return Result.Success();
    }
}
