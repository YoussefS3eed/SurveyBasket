namespace SurveyBasket.Application.Features.Users.Dtos;

public record UserProfileResponseDto(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string Username
);
