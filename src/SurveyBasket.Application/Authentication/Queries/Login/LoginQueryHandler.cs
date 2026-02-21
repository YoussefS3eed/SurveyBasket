using SurveyBasket.Application.Abstractions;
using SurveyBasket.Application.Authentication.Dtos;

namespace SurveyBasket.Application.Authentication.Queries.Login;

public class LoginQueryHandler(IUserRepository userRepository, IJwtProvider jwtProvider)
    : IRequestHandler<LoginQuery, Result<AuthResponseDto>>
{
    public async Task<Result<AuthResponseDto>> Handle(
        LoginQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            return Result.Failure<AuthResponseDto>(Error.Unauthorized);

        var isValidPassword = await userRepository.CheckPasswordAsync(user, request.Password);

        if (!isValidPassword)
            return Result.Failure<AuthResponseDto>(Error.Unauthorized);

        var (token, expiresIn) = jwtProvider.GenerateToken(user);

        var response = new AuthResponseDto(
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            token,
            expiresIn);

        return Result.Success(response);
    }
}