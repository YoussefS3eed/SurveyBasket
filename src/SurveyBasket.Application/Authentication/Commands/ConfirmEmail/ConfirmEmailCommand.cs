namespace SurveyBasket.Application.Authentication.Commands.ConfirmEmail;

public record ConfirmEmailCommand(
    string UserId,
    string Code
) : IRequest<Result>;