using SurveyBasket.Application.Common.Constants;

namespace SurveyBasket.Application.Features.Authentication.Commands.Register;

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
            .Matches(ValidationPatterns.Username)
            .WithMessage("Username can only contain letters, numbers, hyphen and underscore");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(ValidationPatterns.Password)
            .WithMessage("Password must be at least 8 characters, contain at least one uppercase letter, one lowercase letter, one number and one special character");
    }
}