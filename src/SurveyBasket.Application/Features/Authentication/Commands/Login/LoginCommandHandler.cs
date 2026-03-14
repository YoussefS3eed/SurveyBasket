using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Application.Features.Authentication.Dtos;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Authentication.Commands.Login;

internal sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtService jwtService)
    : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(
        LoginCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.GetByUserNameOrEmailAsync(request.EmailOrUserName, cancellationToken) is not { } user)
            return UserErrors.InvalidCredentials;

        if (!await userRepository.CheckPasswordAsync(user, request.Password))
            return UserErrors.InvalidCredentials;

        if (!user.EmailConfirmed)
            return UserErrors.EmailNotConfirmed;

        // Get user roles and permissions
        var (userRoles, userPermissions) = await userRepository.GetUserRolesAndPermissionsAsync(user, cancellationToken);

        // Generate tokens with roles and permissions
        var (token, expiresIn, refreshTokenExpiryDays) =
            jwtService.GenerateToken(user.ToTokenRequest(), userRoles, userPermissions);

        var refreshToken = jwtService.GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        // Persist refresh token (Identity manages its own SaveChanges)
        user.RefreshTokens.Add(new Domain.Entities.RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await userRepository.UpdateAsync(user);

        return Result.Success(new AuthResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.UserName!,
            user.Email!,
            token,
            expiresIn,
            refreshToken,
            refreshTokenExpiration));
    }
}