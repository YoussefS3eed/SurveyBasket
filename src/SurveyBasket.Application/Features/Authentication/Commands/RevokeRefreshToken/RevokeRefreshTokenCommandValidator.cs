namespace SurveyBasket.Application.Features.Authentication.Commands.RevokeRefreshToken;

internal class RevokeRefreshTokenCommandValidator : AbstractValidator<RevokeRefreshTokenCommand>
{
    public RevokeRefreshTokenCommandValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty();

        RuleFor(x => x.RefreshToken)
            .NotEmpty();
    }
}
