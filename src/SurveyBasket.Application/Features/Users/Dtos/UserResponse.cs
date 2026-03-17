namespace SurveyBasket.Application.Features.Users.Dtos;

public record UserResponse(
    string Id,
    string UserName,
    string FirstName,
    string LastName,
    string Email,
    bool IsDisabled,
    IEnumerable<string> Roles
);
