namespace SurveyBasket.Application.Authentication.Commands.RevokeRefreshToken;

public record RevokeRefreshTokenCommand(string Token, string RefreshToken)
    : IRequest<Result>;


