namespace SurveyBasket.Application.Authentication.Commands.RefreshToken;

internal class RevokeRefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RevokeRefreshTokenCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();

        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}
