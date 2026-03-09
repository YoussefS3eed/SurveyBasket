using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Authentication.Commands.RevokeRefreshToken;

public record RevokeRefreshTokenCommand(string Token, string RefreshToken)
    : IRequest<Result>;


