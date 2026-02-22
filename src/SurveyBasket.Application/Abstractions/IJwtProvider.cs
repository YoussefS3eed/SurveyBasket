using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Application.Abstractions;

public interface IJwtProvider
{
    (string token, int expiresIn, int refreshTokenExpiryDays) GenerateToken(ApplicationUser user);
    string GenerateRefreshToken();
    string? ValidateToken(string token);
}