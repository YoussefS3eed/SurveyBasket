namespace SurveyBasket.Application.Errors;

public static class UserErrors
{
    public static readonly Error NotFound = new("User.NotFound", "User not found.");
    public static readonly Error InvalidCredentials = new("User.InvalidCredentials", "Invalid email or password.");
    public static readonly Error InvalidJwtToken = new("User.InvalidJwtToken", "Invalid access token");
    public static readonly Error InvalidRefreshToken = new("User.InvalidRefreshToken", "Invalid refresh token");
}