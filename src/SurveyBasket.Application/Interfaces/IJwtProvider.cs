using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Application.Interfaces;

public interface IJwtProvider
{
    (string token, int expiresIn, int refreshTokenExpiryDays) GenerateToken(ApplicationUser user);
    string GenerateRefreshToken();
    string? ValidateToken(string token);
}