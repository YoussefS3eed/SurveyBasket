namespace SurveyBasket.Application.Authentication.Commands.Login;

internal class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.EmailOrUserName)
            .NotEmpty();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}