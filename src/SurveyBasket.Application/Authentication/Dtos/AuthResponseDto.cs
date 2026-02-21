// SurveyBasket.Application/Authentication/Dtos/AuthResponseDto.cs
namespace SurveyBasket.Application.Authentication.Dtos;

public record AuthResponseDto(
    string Id,
    string? Email,
    string FirstName,
    string LastName,
    string Token,
    int ExpiresIn
);
