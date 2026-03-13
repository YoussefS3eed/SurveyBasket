using SurveyBasket.Application.Common.Constants;

namespace SurveyBasket.Application.Features.Authentication.Commands.ResetPassword;

public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Code)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches(ValidationPatterns.Password)
            .WithMessage("Password must be at least 8 characters and contain uppercase, lowercase, digit, and special character.");
    }
}
