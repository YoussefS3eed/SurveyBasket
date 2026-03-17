namespace SurveyBasket.Application.Features.Users.Commands.UnlockUser;

public record UnlockUserCommand(string Id) : IRequest<Result>;
