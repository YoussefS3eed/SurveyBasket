using SurveyBasket.Domain.Common.Dtos;

namespace SurveyBasket.Application.Common.Interfaces;

public interface IJwtService
{
    (string token, int expiresIn, int refreshTokenExpiryDays) GenerateToken(UserTokenRequest tokenRequest);
    string GenerateRefreshToken();
    string? ValidateToken(string token);
}