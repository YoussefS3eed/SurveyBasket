using SurveyBasket.Application.Interfaces;

namespace SurveyBasket.Application.Authentication.Commands.Login;

internal class LoginCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider)
    : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var isValidPassword = await userRepository.CheckPasswordAsync(user, request.Password);

        if (!isValidPassword)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var (token, expiresIn, refreshTokenExpiryDays) = jwtProvider.GenerateToken(user);
        var refreshToken = jwtProvider.GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);

        user.RefreshTokens.Add(new Domain.Entities.RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await userRepository.UpdateUserAsync(user);

        var response = new AuthResponse(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            token,
            expiresIn,
            refreshToken,
            refreshTokenExpiration);

        return Result.Success(response);
    }
}