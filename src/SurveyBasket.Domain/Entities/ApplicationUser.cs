using Microsoft.AspNetCore.Identity;
using SurveyBasket.Domain.Common;

namespace SurveyBasket.Domain.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    public List<RefreshToken> RefreshTokens { get; set; } = [];

    public static ApplicationUser Create(
        string firstName, string lastName,
        string username, string email)
    => new()
    {
        FirstName = firstName,
        LastName = lastName,
        UserName = username,
        Email = email
    };

    // ✅ Converts to the JWT payload record without exposing Identity
    public UserTokenRequest ToTokenRequest() =>
        new(Id, Email!, FirstName, LastName);
}
