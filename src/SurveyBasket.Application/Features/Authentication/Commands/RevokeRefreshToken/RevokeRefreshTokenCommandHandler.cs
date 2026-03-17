using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Application.Features.Authentication.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Authentication.Commands.RevokeRefreshToken;

internal class RevokeRefreshTokenCommandHandler(IUserRepository userRepository, IJwtService jwtProvider)
    : IRequestHandler<RevokeRefreshTokenCommand, Result>
{
    public async Task<Result> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = jwtProvider.ValidateToken(request.Token);

        if (string.IsNullOrEmpty(userId))
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var user = await userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var storedRefreshToken = user.RefreshTokens
            .SingleOrDefault(rt => rt.Token == request.RefreshToken && rt.IsActive);

        if (storedRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

        storedRefreshToken.RevokedOn = DateTime.UtcNow;

        await userRepository.UpdateAsync(user);

        return Result.Success();
    }
}
