using SurveyBasket.Application.Features.Users.Dtos;

namespace SurveyBasket.Application.Features.Users.Commands.CreateUser;

public record CreateUserCommand(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    IEnumerable<string>? Roles
) : IRequest<Result<UserResponse>>;
