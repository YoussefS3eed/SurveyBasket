using SurveyBasket.Application.Features.Roles.Dtos;

namespace SurveyBasket.Application.Features.Roles.Queries.GetAllRoles;

public record GetAllRolesQuery(bool? IncludeDisabled = false)
    : IRequest<Result<IEnumerable<RoleResponse>>>;
