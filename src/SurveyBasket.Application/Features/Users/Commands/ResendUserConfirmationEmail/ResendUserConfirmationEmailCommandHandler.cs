using SurveyBasket.Application.Common.Extensions;
using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace SurveyBasket.Application.Features.Users.Commands.ResendUserConfirmationEmail;

internal sealed class ResendUserConfirmationEmailCommandHandler(
    IUserRepository userRepository,
    IBackgroundJobService backgroundJobService,
    IApplicationUrlService urlService,
    ILogger<ResendUserConfirmationEmailCommandHandler> logger)
    : IRequestHandler<ResendUserConfirmationEmailCommand, Result>
{
    public async Task<Result> Handle(ResendUserConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        // 1. Find user by email
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        
        // If user not found, still return success (don't reveal if email exists)
        if (user is null)
            return Result.Success();

        // 2. Check if already confirmed
        if (await userRepository.IsEmailConfirmedAsync(user))
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        // 3. Generate confirmation token
        var code = await userRepository.GenerateEmailConfirmationTokenAsync(user);
        code = code.ToBase64UrlEncoded();

        logger.LogInformation(
            "Resending confirmation email to {Email}. Confirmation code generated.", request.Email);

        // 4. Build confirmation URL
        var origin = urlService.GetOrigin();
        var confirmationLink =
            $"{origin}/users/confirm-email?userId={user.Id}&code={code}";

        // 5. Send confirmation email
        backgroundJobService.Enqueue<IEmailService>(emailService =>
            emailService.SendConfirmationEmailAsync(user.Email!, user.FullName, confirmationLink, cancellationToken));

        return Result.Success();
    }
}
