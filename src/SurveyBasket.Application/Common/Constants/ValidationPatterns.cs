namespace SurveyBasket.Application.Common.Constants;

public static class ValidationPatterns
{
    public const string Password =
        "(?=(.*[0-9]))(?=.*[\\!@#$%^&*()\\\\[\\]{}\\-_+=~`|:;\"'<>,./?])(?=.*[a-z])(?=(.*[A-Z]))(?=(.*)).{8,}";

    public const string Username = @"^[a-zA-Z0-9_-]{3,50}$";
}
