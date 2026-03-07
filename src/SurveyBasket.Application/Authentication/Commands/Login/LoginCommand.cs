namespace SurveyBasket.Application.Authentication.Commands.Login;

public record LoginCommand(string EmailOrUserName, string Password)
    : IRequest<Result<AuthResponse>>;