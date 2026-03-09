using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Domain.Common.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SurveyBasket.Infrastructure.Services.Auth;

internal sealed class JwtService(IOptions<JwtOptions> options) : IJwtService
{
    private readonly JwtOptions _options = options.Value;

    // ✅ Takes UserTokenRequest — no ApplicationUser, no Identity reference
    public (string token, int expiresIn, int refreshTokenExpiryDays) GenerateToken(
        UserTokenRequest tokenRequest)
    {
        Claim[] claims =
        [
            new(JwtRegisteredClaimNames.Sub,        tokenRequest.Id),
            new(JwtRegisteredClaimNames.Email,       tokenRequest.Email),
            new(JwtRegisteredClaimNames.GivenName,  tokenRequest.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, tokenRequest.LastName),
            new(JwtRegisteredClaimNames.Jti,         Guid.NewGuid().ToString())
        ];

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            signingCredentials: credentials);

        return (
            new JwtSecurityTokenHandler().WriteToken(token),
            _options.ExpiryMinutes * 60,
            _options.RefreshTokenExpiryDays
        );
    }

    public string GenerateRefreshToken()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

    public string? ValidateToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        try
        {
            handler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = key,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validated);

            return ((JwtSecurityToken)validated)
                .Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
        }
        catch
        {
            return null;
        }
    }
}