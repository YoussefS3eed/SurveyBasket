using SurveyBasket.Application.Features.Roles.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Roles.Queries.GetRoleById;

public class GetRoleByIdQueryHandler(IRoleRepository roleRepository)
    : IRequestHandler<GetRoleByIdQuery, Result<RoleDetailResponse>>
{
    public async Task<Result<RoleDetailResponse>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (role is null)
            return Result.Failure<RoleDetailResponse>(RoleErrors.RoleNotFound(request.Id));

        var permissions = await roleRepository.GetPermissionsForRoleAsync(request.Id, cancellationToken);

        return Result.Success(new RoleDetailResponse(role.Id, role.Name!, role.IsDeleted, permissions));
    }
}
