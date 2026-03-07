namespace SurveyBasket.Application.Errors;

public static class UserErrors
{
    public static readonly Error NotFound =
        new("User.NotFound", "User not found.", 404);

    public static readonly Error InvalidCredentials =
        new("User.InvalidCredentials", "Invalid email or password.", 401);

    public static readonly Error InvalidJwtToken =
        new("User.InvalidJwtToken", "Invalid access token", 401);

    public static readonly Error InvalidRefreshToken =
        new("User.InvalidRefreshToken", "Invalid refresh token", 401);

    public static readonly Error DuplicatedEmail =
    new("User.DuplicatedEmail", "Another user with the same email is already exists", 409);

    public static readonly Error DuplicatedUsername =
        new("User.Conflict", "Another user with the same username already exists", 409);

    public static readonly Error EmailNotConfirmed =
        new("User.EmailNotConfirmed", "Email is not confirmed", 401);

    public static readonly Error InvalidCode =
        new("User.InvalidCode", "Invalid code", 401);

    public static readonly Error DuplicatedConfirmation =
        new("User.DuplicatedConfirmation", "Email already confirmed", 400);

}