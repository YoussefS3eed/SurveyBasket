using SurveyBasket.Application.Features.Roles.Dtos;

namespace SurveyBasket.Application.Features.Roles.Commands.CreateRole;

public record CreateRoleCommand(
    string Name,
    IEnumerable<string> Permissions
) : IRequest<Result<RoleDetailResponse>>;
