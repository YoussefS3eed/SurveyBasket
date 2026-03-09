namespace SurveyBasket.Application.Features.Authentication.Dtos;

public record LoginRequest(
    string EmailOrUserName,
    string Password
);