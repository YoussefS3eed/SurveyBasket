namespace SurveyBasket.Domain.Common.Dtos;

public sealed record UserTokenRequestDto(
    string Id,
    string Email,
    string FirstName,
    string LastName
);
