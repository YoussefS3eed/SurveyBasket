namespace SurveyBasket.Application.Authentication.Dtos;

public record LoginRequest(
    string Email,
    string Password
);