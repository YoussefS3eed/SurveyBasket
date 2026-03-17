namespace SurveyBasket.Domain.Common.Dtos;

public sealed record UserProfileDto(
    string FirstName,
    string LastName,
    string UserName,
    string Email
);
