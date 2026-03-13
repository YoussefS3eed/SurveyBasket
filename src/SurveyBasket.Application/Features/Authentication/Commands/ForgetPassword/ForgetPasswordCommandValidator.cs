namespace SurveyBasket.Application.Features.Authentication.Commands.ForgetPassword;

public sealed class ForgetPasswordCommandValidator : AbstractValidator<ForgetPasswordCommand>
{
    public ForgetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
