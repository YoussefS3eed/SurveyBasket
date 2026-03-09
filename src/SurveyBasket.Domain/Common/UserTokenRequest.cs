namespace SurveyBasket.Domain.Common;

/// <summary>
/// Carries only the claims needed to build a JWT token.
/// Decouples IJwtService from ApplicationUser and the Identity package.
/// </summary>
public sealed record UserTokenRequest(
    string Id,
    string Email,
    string FirstName,
    string LastName
);
