namespace SurveyBasket.Application.Authentication.Dtos;

public record RegisterRequest(
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string Password
);
