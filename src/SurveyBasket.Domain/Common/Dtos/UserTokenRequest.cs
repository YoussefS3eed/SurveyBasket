namespace SurveyBasket.Domain.Common.Dtos;

public sealed record UserTokenRequest(
    string Id,
    string Email,
    string FirstName,
    string LastName
);
