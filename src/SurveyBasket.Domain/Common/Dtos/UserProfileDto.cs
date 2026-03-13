namespace SurveyBasket.Domain.Common.Dtos;

public sealed record UserProfileDto(
    string Email,
    string UserName,
    string FirstName,
    string LastName
);
