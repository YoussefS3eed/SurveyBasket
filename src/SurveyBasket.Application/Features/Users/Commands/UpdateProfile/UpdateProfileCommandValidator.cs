using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Users.Commands.UpdateProfile;

public sealed class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator(IUserRepository userRepository, ICurrentUserService currentUser)
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50)
            .Matches(@"^[a-zA-Z0-9_@.+-]+$")
            .MustAsync(async (userName, ct) =>
            {
                if (string.IsNullOrWhiteSpace(userName))
                    return false;

                var user = await userRepository.GetByUsernameAsync(userName, ct);
                return user is null || user.Id == currentUser.Id;
            })
            .WithMessage("Username '{PropertyValue}' is already taken.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (email, ct) =>
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                var user = await userRepository.GetByEmailAsync(email, ct);
                return user is null || user.Id == currentUser.Id;
            })
            .WithMessage("Email '{PropertyValue}' is already registered.");
    }
}
