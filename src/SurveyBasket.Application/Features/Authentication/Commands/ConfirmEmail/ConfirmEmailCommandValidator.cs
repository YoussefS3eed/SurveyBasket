namespace SurveyBasket.Application.Features.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Code)
            .NotEmpty();
    }
}