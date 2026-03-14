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
        // Validate existing JWT to extract userId
        var userId = jwtService.ValidateToken(request.Token);
        if (string.IsNullOrEmpty(userId))
            return UserErrors.InvalidJwtToken;

        if (await userRepository.GetByIdAsync(userId, cancellationToken) is not { } user)
            return UserErrors.InvalidJwtToken;

        // Validate refresh token && Revoke old token
        var storedToken = user.RefreshTokens
            .SingleOrDefault(rt => rt.Token == request.RefreshToken && rt.IsActive);

        if (storedToken is null)
            return UserErrors.InvalidRefreshToken;

        storedToken.RevokedOn = DateTime.UtcNow;

        // Get user roles and permissions
        var (userRoles, userPermissions) = await userRepository.GetUserRolesAndPermissionsAsync(user, cancellationToken);

        // Generate tokens with roles and permissions
        var (newToken, expiresIn, refreshTokenExpiryDays) =
            jwtService.GenerateToken(user.ToTokenRequest(), userRoles, userPermissions);

        var newRefreshToken = jwtService.GenerateRefreshToken();
        var refreshExpiration = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        user.RefreshTokens.Add(new Domain.Entities.RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshExpiration
        });

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
