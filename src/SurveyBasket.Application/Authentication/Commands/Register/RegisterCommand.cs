namespace SurveyBasket.Application.Authentication.Commands.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Username,
    string Email,
    string Password
) : IRequest<Result>;
