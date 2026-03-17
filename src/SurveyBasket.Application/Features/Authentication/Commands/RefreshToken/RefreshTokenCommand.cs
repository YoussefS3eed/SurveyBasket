using SurveyBasket.Application.Features.Authentication.Dtos;

namespace SurveyBasket.Application.Features.Authentication.Commands.RefreshToken;

public record RefreshTokenCommand(string Token, string RefreshToken)
    : IRequest<Result<AuthResponse>>;


