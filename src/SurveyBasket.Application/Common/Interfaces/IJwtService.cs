using SurveyBasket.Domain.Common.Dtos;

namespace SurveyBasket.Application.Common.Interfaces;

public interface IJwtService
{
    (string token, int expiresIn, int refreshTokenExpiryDays) GenerateToken(UserTokenRequest tokenRequest, IEnumerable<string> roles, IEnumerable<string> permissions, string? securityStamp = null);
    string GenerateRefreshToken();
    string? ValidateToken(string token);
    (string? UserId, bool IsStampValid)? ValidateTokenWithSecurityStamp(string token, string currentSecurityStamp);
}