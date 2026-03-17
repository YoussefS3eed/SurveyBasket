namespace SurveyBasket.Application.Features.Users.Dtos;

public record UserProfileResponse(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string Username
);
