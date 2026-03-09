namespace SurveyBasket.Application.Features.Authentication.Dtos;

public record AuthResponse(
    string Id,
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string Token,
    int ExpiresIn,
    string RefreshToken,
    DateTime RefreshTokenExpiration
);
