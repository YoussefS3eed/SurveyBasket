namespace SurveyBasket.Application.Authentication.Dtos;

public record ConfirmEmailRequest(
    string UserId,
    string Code
);
