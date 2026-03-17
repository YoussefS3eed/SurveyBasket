namespace SurveyBasket.Domain.Errors;

public static class UserErrors
{
    public static Error NotFound(object? key = null) =>
        key is null
            ? Error.NotFound("User.NotFound", "User not found.")
            : Error.NotFound("User.NotFound", $"User with id '{key}' was not found.");

    public static readonly Error InvalidCredentials =
        Error.Unauthorized("User.InvalidCredentials", "Invalid Email/Username or Password.");

    public static readonly Error DisabledUser =
        Error.Unauthorized("User.DisabledUser", "User account is disabled. Contact your administrator.");

    public static readonly Error LockedUser =
        Error.Unauthorized("User.LockedUser", "User account is locked. Contact your administrator.");

    public static readonly Error InvalidJwtToken =
        Error.Unauthorized("User.InvalidJwtToken", "Invalid access token.");

    public static readonly Error InvalidRefreshToken =
        Error.Unauthorized("User.InvalidRefreshToken", "Invalid refresh token.");

    public static readonly Error EmailNotConfirmed =
        Error.Unauthorized("User.EmailNotConfirmed", "Email is not confirmed.");

    public static readonly Error InvalidCode =
        Error.Unauthorized("User.InvalidCode", "Invalid or expired confirmation code.");

    public static readonly Error DuplicatedConfirmation =
        Error.Validation("User.DuplicatedConfirmation", "Email is already confirmed.");

    public static readonly Error InvalidRoles =
        Error.Validation("User.InvalidRoles", "One or more roles are invalid.");

    // ✅ One canonical error per concept — no duplicates
    public static Error DuplicatedEmail(string? email = null) =>
        email is null
            ? Error.Conflict("User.DuplicatedEmail", "A user with this email already exists.")
            : Error.Conflict("User.DuplicatedEmail", $"Email '{email}' is already registered.");

    public static Error DuplicatedUsername(string? username = null) =>
        username is null
            ? Error.Conflict("User.DuplicatedUsername", "A user with this username already exists.")
            : Error.Conflict("User.DuplicatedUsername", $"Username '{username}' is already taken.");
}