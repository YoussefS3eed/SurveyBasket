using SurveyBasket.Application.Common.Extensions;
using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Application.Features.Authentication.Dtos;
using SurveyBasket.Domain.Common.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace SurveyBasket.Application.Features.Users.Commands.VerifyProfileEmail;

public sealed class VerifyProfileEmailCommandHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUser,
    IJwtService jwtService,
    ILogger<VerifyProfileEmailCommandHandler> logger)
    : IRequestHandler<VerifyProfileEmailCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(VerifyProfileEmailCommand request, CancellationToken cancellationToken)
    {
        // Get authenticated user from token
        var authenticatedUserId = currentUser.Id;
        if (string.IsNullOrEmpty(authenticatedUserId))
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        // Verify the code and get userId from the code itself
        var (isValid, newEmail, userId) = await userRepository.VerifyEmailCodeAsync(request.Code, cancellationToken);

        if (!isValid || newEmail is null || userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCode);

        // Ensure token userId matches code userId (prevent cross-user verification)
        if (userId != authenticatedUserId)
        {
            logger.LogWarning("User {AuthenticatedUserId} attempted to verify email for user {UserId}", authenticatedUserId, userId);
            return Result.Failure<AuthResponse>(UserErrors.InvalidCode);
        }

        // Get user
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.NotFound(userId));

        // Confirm the email
        user.Email = newEmail;
        user.EmailConfirmed = true;

        // Revoke all existing refresh tokens (single session per user - like login)
        foreach (var existingToken in user.RefreshTokens.Where(rt => rt.IsActive))
        {
            existingToken.RevokedOn = DateTime.UtcNow;
        }

        // Generate NEW security stamp (invalidates all existing JWT tokens)
        user.SecurityStamp = Guid.NewGuid().ToString();

        var updateResult = await userRepository.UpdateAsync(user);
        if (updateResult.IsFailure)
            return Result.Failure<AuthResponse>(updateResult.Error);

        logger.LogInformation("Email verified and confirmed for user {UserId}. All previous sessions invalidated.", userId);

        // Auto-login: Generate JWT tokens with NEW security stamp
        var (token, expiresIn, refreshTokenExpiryDays) = jwtService.GenerateToken(
            new UserTokenRequest(user.Id, user.Email!, user.FirstName, user.LastName),
            await userRepository.GetRolesAsync(user),
            [],
            user.SecurityStamp);

        var refreshToken = jwtService.GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        // Save new refresh token
        user.RefreshTokens.Add(new Domain.Entities.RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await userRepository.UpdateAsync(user);

        logger.LogInformation("User {UserId} auto-logged in after email verification with new session.", userId);

        return new AuthResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.UserName!,
            user.Email!,
            token,
            expiresIn,
            refreshToken,
            refreshTokenExpiration);
    }
}
