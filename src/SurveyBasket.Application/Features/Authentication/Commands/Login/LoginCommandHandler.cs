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
        // 1. Find user
        var user = await userRepository.GetByUserNameOrEmailAsync(
            request.EmailOrUserName, cancellationToken);

        if (user is null)
            return UserErrors.InvalidCredentials;

        // 2. Check password
        var isValidPassword = await userRepository.CheckPasswordAsync(user, request.Password);
        if (!isValidPassword)
            return UserErrors.InvalidCredentials;

        // 3. Check email confirmation
        var isEmailConfirmed = await userRepository.IsEmailConfirmedAsync(user);
        if (!isEmailConfirmed)
            return UserErrors.EmailNotConfirmed;

        // 4. Generate tokens
        // ✅ ToTokenRequest() — converts ApplicationUser to JWT payload record
        var (token, expiresIn, refreshTokenExpiryDays) =
            jwtService.GenerateToken(user.ToTokenRequest());

        var refreshToken = jwtService.GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        // 5. Persist refresh token (Identity manages its own SaveChanges)
        user.RefreshTokens.Add(new Domain.Entities.RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await userRepository.UpdateAsync(user);

        // 6. Build response
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