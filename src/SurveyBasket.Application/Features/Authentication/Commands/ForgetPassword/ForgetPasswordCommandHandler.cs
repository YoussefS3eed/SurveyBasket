using Microsoft.Extensions.Logging;
using SurveyBasket.Application.Common.Extensions;
using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Authentication.Commands.ForgetPassword;

public sealed class ForgetPasswordCommandHandler(
    IUserRepository userRepository,
    IBackgroundJobService backgroundJobService,
    IApplicationUrlService urlService,
    ILogger<ForgetPasswordCommandHandler> logger)
    : IRequestHandler<ForgetPasswordCommand, Result>
{
    public async Task<Result> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.GetByEmailAsync(request.Email, cancellationToken) is not { } user)
            return Result.Success();

        if (!user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailNotConfirmed);

        var code = await userRepository.GeneratePasswordResetTokenAsync(user);
        code = code.ToBase64UrlEncoded();

        logger.LogInformation("Password reset code for {Email}: {Code}", request.Email, code);

        var origin = urlService.GetOrigin();
        var resetLink =
            $"{origin}/auth/forgetPassword?email={user.Email}&code={code}";

        backgroundJobService.Enqueue<IEmailService>(emailService =>
            emailService.SendPasswordResetEmailAsync(user.Email!, user.FullName, resetLink, cancellationToken));

        return Result.Success();
    }
}
