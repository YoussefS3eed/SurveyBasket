namespace SurveyBasket.Application.Features.Roles.Commands.UpdateRole;

public record UpdateRoleCommand(
    string? Id,
    string Name,
    IEnumerable<string> Permissions
) : IRequest<Result>;
