using SurveyBasket.Application.Abstractions;

namespace SurveyBasket.Application.Authentication.Commands.RefreshToken;

internal class RefreshTokenCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider)
    : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = jwtProvider.ValidateToken(request.Token);

        if (string.IsNullOrEmpty(userId))
            return Result.Failure<AuthResponse>(Error.Unauthorized with { Description = "Invalid access token" });

        var user = await userRepository.GetByIdAsync(userId);

        if (user is null)
            return Result.Failure<AuthResponse>(Error.Unauthorized with { Description = "Invalid access token" });

        var storedRefreshToken = user.RefreshTokens
            .SingleOrDefault(rt => rt.Token == request.RefreshToken && rt.IsActive);

        if (storedRefreshToken is null)
            return Result.Failure<AuthResponse>(Error.Unauthorized with { Description = "Invalid access token" });

        storedRefreshToken.RevokedOn = DateTime.UtcNow;

        var (newToken, expiresIn, refreshTokenExpiryDays) = jwtProvider.GenerateToken(user);
        var newRefreshToken = jwtProvider.GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        user.RefreshTokens.Add(new Domain.Entities.RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await userRepository.UpdateUserAsync(user);

        var response = new AuthResponse(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            newToken,
            expiresIn,
            newRefreshToken,
            refreshTokenExpiration);

        return Result.Success(response);
    }
}
