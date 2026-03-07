using Microsoft.Extensions.Logging;
using SurveyBasket.Application.Interfaces;

namespace SurveyBasket.Application.Authentication.Commands.Login;

internal class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtProvider jwtProvider,
    ILogger<LoginCommandHandler> logger)
    : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.GetByUserNameOrEmailAsync(request.EmailOrUserName) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        if (!await userRepository.CheckPasswordAsync(user, request.Password))
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        if (!await userRepository.IsEmailConfirmedAsync(user))
            return Result.Failure<AuthResponse>(UserErrors.EmailNotConfirmed);

        var (token, expiresIn, refreshTokenExpiryDays) = jwtProvider.GenerateToken(user);
        var refreshToken = jwtProvider.GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        await userRepository.AddRefreshTokenAsync(user, new Domain.Entities.RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        var response = new AuthResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.UserName,
            user.Email,
            token,
            expiresIn,
            refreshToken,
            refreshTokenExpiration);

        logger.LogInformation("User {EmailOrUserName} logged in successfully", request.EmailOrUserName);
        return Result.Success(response);
    }
}