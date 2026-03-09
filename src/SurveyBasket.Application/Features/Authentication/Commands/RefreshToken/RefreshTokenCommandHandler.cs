using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Application.Features.Authentication.Dtos;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Authentication.Commands.RefreshToken;

internal sealed class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    IJwtService jwtService)
    : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(
        RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate existing JWT to extract userId
        var userId = jwtService.ValidateToken(request.Token);
        if (string.IsNullOrEmpty(userId))
            return UserErrors.InvalidJwtToken;

        // 2. Load user
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return UserErrors.InvalidJwtToken;

        // 3. Validate refresh token
        var storedToken = user.RefreshTokens
            .SingleOrDefault(rt => rt.Token == request.RefreshToken && rt.IsActive);

        if (storedToken is null)
            return UserErrors.InvalidRefreshToken;

        // 4. Revoke old token
        storedToken.RevokedOn = DateTime.UtcNow;

        // 5. Issue new tokens
        // ✅ ToTokenRequest() — no Identity types in jwt call
        var (newToken, expiresIn, refreshTokenExpiryDays) =
            jwtService.GenerateToken(user.ToTokenRequest());

        var newRefreshToken = jwtService.GenerateRefreshToken();
        var refreshExpiration = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        user.RefreshTokens.Add(new Domain.Entities.RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshExpiration
        });

        // 6. Persist — Identity manages its own SaveChanges
        await userRepository.UpdateAsync(user);

        return Result.Success(new AuthResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.UserName!,
            user.Email!,
            newToken,
            expiresIn,
            newRefreshToken,
            refreshExpiration));
    }
}
