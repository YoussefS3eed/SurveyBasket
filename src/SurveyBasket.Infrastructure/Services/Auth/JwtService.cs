using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Domain.Common.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SurveyBasket.Infrastructure.Services.Auth;

internal sealed class JwtService(IOptions<JwtOptions> options, ILogger<JwtService> logger) : IJwtService
{
    private readonly JwtOptions _options = options.Value;

    public (string token, int expiresIn, int refreshTokenExpiryDays) GenerateToken(
        UserTokenRequest tokenRequest,
        IEnumerable<string> roles,
        IEnumerable<string> permissions,
        string? securityStamp = null)
    {
        var claimsList = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, tokenRequest.Id),
            new(JwtRegisteredClaimNames.Email, tokenRequest.Email),
            new(JwtRegisteredClaimNames.GivenName, tokenRequest.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, tokenRequest.LastName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Add security stamp if provided
        if (!string.IsNullOrEmpty(securityStamp))
        {
            claimsList.Add(new Claim("securityStamp", securityStamp));
        }

        claimsList.Add(new Claim("roles", JsonSerializer.Serialize(roles), JsonClaimValueTypes.JsonArray));
        claimsList.Add(new Claim("permissions", JsonSerializer.Serialize(permissions), JsonClaimValueTypes.JsonArray));

        Claim[] claims = [.. claimsList];

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
                ValidateIssuer = true,
                ValidIssuer = _options.Issuer,
                ValidateAudience = true,
                ValidAudience = _options.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return ((JwtSecurityToken)validatedToken)
                .Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
        }
        catch (SecurityTokenException ex)
        {
            logger.LogWarning("Token validation failed: {Reason}", ex.Message);
            return null;
        }
    }

    public (string? UserId, bool IsStampValid)? ValidateTokenWithSecurityStamp(string token, string currentSecurityStamp)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));

        try
        {
            handler.ValidateToken(token, new TokenValidationParameters
            {
                IssuerSigningKey = key,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidIssuer = _options.Issuer,
                ValidateAudience = true,
                ValidAudience = _options.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(c => c.Type == JwtRegisteredClaimNames.Sub).Value;
            
            // Validate security stamp
            var stampClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "securityStamp");
            var isStampValid = stampClaim?.Value == currentSecurityStamp;

            return (userId, isStampValid);
        }
        catch (SecurityTokenException ex)
        {
            logger.LogWarning("Token validation failed: {Reason}", ex.Message);
            return null;
        }
    }
}