using SurveyBasket.Application.Features.Roles.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Roles.Queries.GetAllRoles;

public class GetAllRolesQueryHandler(IRoleRepository roleRepository)
    : IRequestHandler<GetAllRolesQuery, Result<IEnumerable<RoleResponse>>>
{
    public async Task<Result<IEnumerable<RoleResponse>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await roleRepository.GetAllAsync(request.IncludeDisabled ?? false, cancellationToken);

        return Result.Success(roles.Adapt<IEnumerable<RoleResponse>>());
    }
}
