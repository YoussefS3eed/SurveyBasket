namespace SurveyBasket.Application.Features.Authentication.Commands.ResendConfirmationEmail;

public class ResendConfirmationEmailCommandValidator : AbstractValidator<ResendConfirmationEmailCommand>
{
    public ResendConfirmationEmailCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
