using SurveyBasket.Application.Common.Extensions;
using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace SurveyBasket.Application.Features.Users.Commands.ResendProfileVerificationCode;

public sealed class ResendProfileVerificationCodeCommandHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUser,
    IBackgroundJobService backgroundJobService,
    ILogger<ResendProfileVerificationCodeCommandHandler> logger)
    : IRequestHandler<ResendProfileVerificationCodeCommand, Result>
{
    public async Task<Result> Handle(ResendProfileVerificationCodeCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.Id!;

        // Get current user
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result.Failure(UserErrors.NotFound(userId));

        // Check if email is confirmed - no need to resend if already confirmed
        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        // Get pending verification code info
        var pendingVerification = await userRepository.GetPendingEmailVerificationAsync(userId, user.Email!, cancellationToken);

        if (pendingVerification == null)
        {
            // No pending verification - this shouldn't happen, generate new code
            var verificationCode = GenerateVerificationCode();

            await userRepository.SetEmailVerificationCodeAsync(
                userId,
                verificationCode,
                user.Email!,
                DateTime.UtcNow.AddMinutes(5),
                cancellationToken);

            logger.LogInformation("New verification code generated for user {UserId}", userId);

            backgroundJobService.Enqueue<IEmailService>(emailService =>
                emailService.SendEmailVerificationCodeAsync(user.Email!, user.FullName, verificationCode, cancellationToken));

            return Result.Success();
        }

        // Generate new code (invalidate old one)
        var newVerificationCode = GenerateVerificationCode();

        await userRepository.SetEmailVerificationCodeAsync(
            userId,
            newVerificationCode,
            user.Email!,
            DateTime.UtcNow.AddMinutes(5),
            cancellationToken);

        logger.LogInformation("Verification code resent to {Email} for user {UserId}", user.Email, userId);

        backgroundJobService.Enqueue<IEmailService>(emailService =>
            emailService.SendEmailVerificationCodeAsync(user.Email!, user.FullName, newVerificationCode, cancellationToken));

        return Result.Success();
    }

    private static string GenerateVerificationCode()
    {
        // Generate a 6-digit numeric code using cryptographic random
        var randomBytes = new byte[4];
        RandomNumberGenerator.Fill(randomBytes);
        var random = new Random(BitConverter.ToInt32(randomBytes, 0));
        return random.Next(100000, 999999).ToString();
    }
}
