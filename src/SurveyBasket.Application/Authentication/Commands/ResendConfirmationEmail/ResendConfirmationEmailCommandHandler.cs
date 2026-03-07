using Microsoft.Extensions.Logging;
using SurveyBasket.Application.Interfaces;

namespace SurveyBasket.Application.Authentication.Commands.ResendConfirmationEmail;

public class ResendConfirmationEmailCommandHandler(
    IUserRepository userRepository,
    IEmailService emailService,
    ILogger<ResendConfirmationEmailCommandHandler> logger)
    : IRequestHandler<ResendConfirmationEmailCommand, Result>
{
    public async Task<Result> Handle(ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.GetByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (await userRepository.IsEmailConfirmedAsync(user))
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        var code = await userRepository.GenerateEmailConfirmationTokenAsync(user);
        code = code.ToBase64UrlEncoded();

        logger.LogInformation("Resent confirmation code for user {Email}: {Code}", request.Email, code);

        await emailService.SendConfirmationEmailAsync(user, code, cancellationToken);

        return Result.Success();
    }
}