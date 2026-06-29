namespace SurveyBasket.Application.Features.Users.Dtos;

public record UserResponseDto(
    string Id,
    string UserName,
    string FirstName,
    string LastName,
    string Email,
    bool IsDisabled,
    IEnumerable<string> Roles
);
