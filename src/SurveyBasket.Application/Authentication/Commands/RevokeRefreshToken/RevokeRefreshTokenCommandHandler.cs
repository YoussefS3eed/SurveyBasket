using SurveyBasket.Application.Interfaces;

namespace SurveyBasket.Application.Authentication.Commands.RevokeRefreshToken;

internal class RevokeRefreshTokenCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider)
    : IRequestHandler<RevokeRefreshTokenCommand, Result>
{
    public async Task<Result> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = jwtProvider.ValidateToken(request.Token);

        if (string.IsNullOrEmpty(userId))
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var user = await userRepository.GetByIdAsync(userId);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var storedRefreshToken = user.RefreshTokens
            .SingleOrDefault(rt => rt.Token == request.RefreshToken && rt.IsActive);

        if (storedRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

        storedRefreshToken.RevokedOn = DateTime.UtcNow;

        await userRepository.UpdateUserAsync(user);

        return Result.Success();
    }
}
