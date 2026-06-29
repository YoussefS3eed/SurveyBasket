namespace SurveyBasket.Application.Features.Authentication.Dtos;

public record AuthResponseDto(
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
