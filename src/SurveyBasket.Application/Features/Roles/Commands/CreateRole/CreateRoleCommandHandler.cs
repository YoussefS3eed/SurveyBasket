using SurveyBasket.Application.Common.Contracts;
using SurveyBasket.Application.Features.Roles.Dtos;
using SurveyBasket.Domain.Entities;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Roles.Commands.CreateRole;

public class CreateRoleCommandHandler(IRoleRepository roleRepository)
    : IRequestHandler<CreateRoleCommand, Result<RoleDetailResponse>>
{
    public async Task<Result<RoleDetailResponse>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        // Check if role exists
        var exists = await roleRepository.ExistsByNameExceptIdAsync(request.Name, null, cancellationToken);
        if (exists)
            return Result.Failure<RoleDetailResponse>(RoleErrors.DuplicateRole(request.Name));

        // Validate permissions
        var allowedPermissions = Permissions.GetAllPermissions();
        if (request.Permissions.Except(allowedPermissions).Any())
            return Result.Failure<RoleDetailResponse>(RoleErrors.InvalidPermissions);

        // Create role
        var role = new ApplicationRole
        {
            Name = request.Name,
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        var addResult = await roleRepository.AddAsync(role, request.Permissions, cancellationToken);
        if (addResult.IsFailure)
            return Result.Failure<RoleDetailResponse>(addResult.Error);

        return Result.Success(new RoleDetailResponse(role.Id, role.Name!, role.IsDeleted, request.Permissions));
    }
}
