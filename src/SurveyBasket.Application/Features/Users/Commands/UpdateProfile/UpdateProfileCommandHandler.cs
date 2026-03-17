using SurveyBasket.Application.Common.Extensions;
using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace SurveyBasket.Application.Features.Users.Commands.UpdateProfile;

public sealed class UpdateProfileCommandHandler(
    IUserRepository userRepository,
    ICurrentUserService currentUser,
    IBackgroundJobService backgroundJobService,
    ILogger<UpdateProfileCommandHandler> logger)
    : IRequestHandler<UpdateProfileCommand, Result>
{
    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.Id!;

        // Get current user
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return Result.Failure(UserErrors.NotFound(userId));

        // Check if email is being changed
        var emailChanged = user.Email != request.Email;

        // Update user properties
        user.UserName = request.UserName;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        if (emailChanged)
        {
            // Check if new email already exists
            if (await userRepository.IsEmailExistsAsync(request.Email, userId, cancellationToken))
                return Result.Failure(UserErrors.DuplicatedEmail(request.Email));

            // Mark email as unconfirmed and regenerate security stamp (invalidates old tokens)
            user.Email = request.Email;
            user.EmailConfirmed = false;
            user.SecurityStamp = Guid.NewGuid().ToString();

            // Save user changes
            var updateResult = await userRepository.UpdateAsync(user);
            if (updateResult.IsFailure)
                return updateResult;

            // Generate 6-digit verification code (5 minutes expiry)
            var verificationCode = GenerateVerificationCode();

            // Store the code and invalidate any existing codes
            await userRepository.SetEmailVerificationCodeAsync(
                userId,
                request.Email,
                verificationCode,
                DateTime.UtcNow.AddMinutes(5),
                cancellationToken);

            // Send verification code email
            logger.LogInformation(
                "Profile email change initiated for user {UserId}: {OldEmail} -> {NewEmail}. Verification code generated. Old tokens invalidated.",
                userId, user.Email, request.Email);

            backgroundJobService.Enqueue<IEmailService>(emailService =>
                emailService.SendEmailVerificationCodeAsync(request.Email, user.FullName, verificationCode, cancellationToken));

            // Return success with redirect URL to login page
            return Result.Success(new { 
                RedirectUrl = "/login", 
                NewEmail = request.Email, 
                TokenInvalidated = true,
                RequiresVerification = true,
                Message = "Please login with your verification code sent to your new email"
            });
        }

        // No email change - update UserName if changed
        if (user.UserName != request.UserName)
        {
            // Check if new username already exists
            if (await userRepository.IsUsernameExistsAsync(request.UserName, userId, cancellationToken))
                return Result.Failure(UserErrors.DuplicatedUsername(request.UserName));
        }

        // Save changes
        var saveResult = await userRepository.UpdateAsync(user);
        if (saveResult.IsFailure)
            return saveResult;

        logger.LogInformation("Profile updated for user {UserId}", userId);

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
