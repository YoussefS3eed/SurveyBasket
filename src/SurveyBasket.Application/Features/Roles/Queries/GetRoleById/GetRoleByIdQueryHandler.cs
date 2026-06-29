using SurveyBasket.Application.Features.Roles.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Roles.Queries.GetRoleById;

public class GetRoleByIdQueryHandler(IRoleRepository roleRepository)
    : IRequestHandler<GetRoleByIdQuery, Result<RoleDetailResponseDto>>
{
    public async Task<Result<RoleDetailResponseDto>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (role is null)
            return Result.Failure<RoleDetailResponseDto>(RoleErrors.RoleNotFound(request.Id));

        var permissions = await roleRepository.GetPermissionsForRoleAsync(request.Id, cancellationToken);

        return Result.Success(new RoleDetailResponseDto(role.Id, role.Name!, role.IsDeleted, permissions));
    }
}
