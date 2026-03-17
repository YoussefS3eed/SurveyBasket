using SurveyBasket.Application.Common.Constants;

namespace SurveyBasket.Application.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
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

        //RuleFor(x => x.Roles)
        //    .NotEmpty().WithMessage("At least one role is required.");
    }
}
