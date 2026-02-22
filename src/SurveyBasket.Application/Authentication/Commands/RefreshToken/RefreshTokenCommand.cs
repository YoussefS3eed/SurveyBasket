namespace SurveyBasket.Application.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(string Token, string RefreshToken)
    : IRequest<Result<AuthResponse>>;


