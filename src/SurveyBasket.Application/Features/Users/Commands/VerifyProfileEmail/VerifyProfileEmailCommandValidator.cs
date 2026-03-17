namespace SurveyBasket.Application.Features.Users.Commands.VerifyProfileEmail;

public sealed class VerifyProfileEmailCommandValidator : AbstractValidator<VerifyProfileEmailCommand>
{
    public VerifyProfileEmailCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .Length(6)
            .Matches(@"^\d{6}$")
            .WithMessage("Code must be a 6-digit number.");
    }
}
