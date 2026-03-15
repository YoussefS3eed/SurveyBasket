namespace SurveyBasket.Application.Features.Roles.Commands.ToggleRoleStatus;

public record ToggleRoleStatusCommand(string Id) : IRequest<Result>;
