using SurveyBasket.Application.Common.Constants;

namespace SurveyBasket.Application.Features.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 100);

        RuleFor(x => x.UserName)
            .NotEmpty()
            .Length(3, 50)
            .Matches(ValidationPatterns.Username)
            .WithMessage("Username can only contain letters, numbers, hyphen and underscore");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
