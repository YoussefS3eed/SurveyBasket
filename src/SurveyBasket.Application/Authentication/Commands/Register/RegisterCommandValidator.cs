namespace SurveyBasket.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.Username)
            .NotEmpty()
            .Length(3, 50)
            .Matches(RegexPatterns.Username)
            .WithMessage("Username can only contain letters, numbers, hyphen and underscore");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password must be at least 8 characters, contain at least one uppercase letter, one lowercase letter, one number and one special character");
    }
}