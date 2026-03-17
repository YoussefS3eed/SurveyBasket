using Microsoft.Extensions.Logging;
using SurveyBasket.Application.Common.Extensions;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Authentication.Commands.ResetPassword;

public sealed class ResetPasswordCommandHandler(
    IUserRepository userRepository,
    ILogger<ResetPasswordCommandHandler> logger)
    : IRequestHandler<ResetPasswordCommand, Result>
{
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || !user.EmailConfirmed)
            return Result.Failure(UserErrors.InvalidCode);

        string decodedCode;
        try
        {
            decodedCode = request.Code.FromBase64UrlEncoded();
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }

        var result = await userRepository.ResetPasswordAsync(user, decodedCode, request.NewPassword);

        if (result.IsSuccess)
        {
            logger.LogInformation("Password reset successfully for {Email}", request.Email);
            return Result.Success();
        }

        return result;
    }
}
