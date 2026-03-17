namespace SurveyBasket.Application.Features.Users.Commands.UpdateProfile;

public sealed record UpdateProfileCommand(
    string UserName,
    string Email,
    string FirstName,
    string LastName
) : IRequest<Result>;
