using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Users.Commands.UpdateProfile;

public sealed record UpdateProfileCommand(
    string FirstName,
    string LastName
) : IRequest<Result>;
