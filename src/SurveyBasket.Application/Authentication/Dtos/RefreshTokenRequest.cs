namespace SurveyBasket.Application.Authentication.Dtos;

public record RefreshTokenRequest(
    string Token,
    string RefreshToken
);
