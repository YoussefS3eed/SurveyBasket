namespace SurveyBasket.Application.Authentication.Dtos;

public record LoginRequest(
    string EmailOrUserName,
    string Password
);