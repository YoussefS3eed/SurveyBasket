using SurveyBasket.Application.Common.Constants;

namespace SurveyBasket.Application.Features.Users.Commands.ChangePassword;

public sealed class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches(ValidationPatterns.Password)
            .WithMessage("Password must be at least 8 characters and contain uppercase, lowercase, digit, and special character.")
            .NotEqual(x => x.CurrentPassword)
            .WithMessage("New password cannot be the same as the current password.");
    }
}
