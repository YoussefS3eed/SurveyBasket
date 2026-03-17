using Microsoft.Extensions.Logging;
using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Application.Features.Authentication.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Authentication.Commands.Login;

internal sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtService jwtService,
    ILogger<LoginCommandHandler> logger)
    : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(
        LoginCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.GetByUserNameOrEmailAsync(request.EmailOrUserName, cancellationToken) is not { } user)
            return UserErrors.InvalidCredentials;

        if (user.IsDisabled)
            return Result.Failure<AuthResponse>(UserErrors.DisabledUser);

        // If verification code is provided, verify it first
        if (!string.IsNullOrEmpty(request.VerificationCode))
        {
            // Verify the code (this will also mark it as used)
            var (isValid, newEmail, userId) = await userRepository.VerifyEmailCodeAsync(request.VerificationCode, cancellationToken);

            if (!isValid || newEmail is null || userId != user.Id)
            {
                logger.LogWarning("Invalid verification code provided for user {UserId}", user.Id);
                return Result.Failure<AuthResponse>(UserErrors.InvalidCode);
            }

            // Confirm the email
            user.Email = newEmail;
            user.EmailConfirmed = true;

            logger.LogInformation("Email verified and confirmed for user {UserId} during login", user.Id);
        }
        else
        {
            // Normal login - check if email is confirmed
            if (!user.EmailConfirmed)
                return UserErrors.EmailNotConfirmed;
        }

        if (!await userRepository.CheckPasswordAsync(user, request.Password, false, true))
            return UserErrors.InvalidCredentials;

        // Get user roles and permissions
        var (userRoles, userPermissions) = await userRepository.GetUserRolesAndPermissionsAsync(user, cancellationToken);

        // Revoke all existing refresh tokens (single session per user)
        foreach (var existingToken in user.RefreshTokens.Where(rt => rt.IsActive))
        {
            existingToken.RevokedOn = DateTime.UtcNow;
        }

        // Generate NEW security stamp (invalidates all existing JWT tokens)
        user.SecurityStamp = Guid.NewGuid().ToString();

        // Generate tokens with roles and permissions
        var (token, expiresIn, refreshTokenExpiryDays) =
            jwtService.GenerateToken(user.ToTokenRequest(), userRoles, userPermissions, user.SecurityStamp);

        var refreshToken = jwtService.GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        // Persist new refresh token
        user.RefreshTokens.Add(new Domain.Entities.RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await userRepository.UpdateAsync(user);

        logger.LogInformation("User {UserId} logged in. All previous sessions have been invalidated.", user.Id);

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