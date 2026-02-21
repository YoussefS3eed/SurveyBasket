// SurveyBasket.Application/Authentication/Dtos/LoginRequestDto.cs
namespace SurveyBasket.Application.Authentication.Dtos;

public record LoginRequestDto(
    string Email,
    string Password
);