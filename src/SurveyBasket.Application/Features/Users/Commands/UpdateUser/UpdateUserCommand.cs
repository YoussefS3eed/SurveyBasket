namespace SurveyBasket.Application.Features.Users.Commands.UpdateUser;

public record UpdateUserCommand(
    string? Id,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    IList<string>? Roles
) : IRequest<Result>;
