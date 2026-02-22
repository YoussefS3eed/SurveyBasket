namespace SurveyBasket.Application.Authentication.Commands.Login;

public record LoginCommand(string Email, string Password)
    : IRequest<Result<AuthResponse>>;