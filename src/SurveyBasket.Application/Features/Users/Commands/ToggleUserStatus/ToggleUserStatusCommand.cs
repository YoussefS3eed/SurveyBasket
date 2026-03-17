namespace SurveyBasket.Application.Features.Users.Commands.ToggleUserStatus;

public record ToggleUserStatusCommand(string Id) : IRequest<Result>;
