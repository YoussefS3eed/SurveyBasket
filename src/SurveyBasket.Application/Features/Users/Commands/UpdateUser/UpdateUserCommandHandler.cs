using Microsoft.Extensions.Logging;
using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Application.Features.Roles.Queries.GetAllRoles;
using SurveyBasket.Domain.Interfaces.Repositories;
using System.Security.Cryptography;

namespace SurveyBasket.Application.Features.Users.Commands.UpdateUser;

internal sealed class UpdateUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    ISender sender,
    IBackgroundJobService backgroundJobService,
    ILogger<UpdateUserCommandHandler> logger)
    : IRequestHandler<UpdateUserCommand, Result>
{
    public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        // Get user
        if (await userRepository.GetByIdAsync(request.Id!, cancellationToken) is not { } user)
            return Result.Failure(UserErrors.NotFound(request.Id));

        // Check if email is being changed
        var emailChanged = user.Email != request.Email;
        var emailNotConfirmed = !user.EmailConfirmed;

        if (emailChanged)
        {
            if (await userRepository.IsEmailExistsAsync(request.Email, request.Id!, cancellationToken))
                return Result.Failure(UserErrors.DuplicatedEmail(request.Email));
        }

        // Validate roles (if provided)
        if (request.Roles != null && request.Roles.Any())
        {
            var rolesResult = await sender.Send(new GetAllRolesQuery(), cancellationToken);
            if (rolesResult.IsFailure)
                return Result.Failure(rolesResult.Error);

            if (request.Roles.Except([.. rolesResult.Value.Select(r => r.Name)]).Any())
                return Result.Failure(UserErrors.InvalidRoles);
        }

        // Update user properties
        user.UserName = request.UserName;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;

        var oldEmail = user.Email;

        if (emailChanged)
        {
            // Scenario 1: Admin changes email
            // Update email and mark as unconfirmed
            user.Email = request.Email;
            user.EmailConfirmed = false;

            // Save user changes
            var updateResult = await userRepository.UpdateAsync(user);
            if (updateResult.IsFailure)
                return updateResult;

            // Generate temporary password
            var temporaryPassword = GenerateTemporaryPassword();

            // Generate confirmation link
            var confirmEmailLink = $"/account/confirm-email?userId={user.Id}&email={Uri.EscapeDataString(request.Email)}&code={Uri.EscapeDataString(temporaryPassword)}";

            // Send email with UserName, Temporary Password, and Verify Link
            logger.LogInformation(
                "Admin changed email for user {UserId}: {OldEmail} -> {NewEmail}. Confirmation email sent.",
                request.Id, oldEmail, request.Email);

            backgroundJobService.Enqueue<IEmailService>(emailService =>
                emailService.SendAdminEmailChangeConfirmationAsync(
                    request.Email,
                    user.FullName,
                    user.UserName!,
                    temporaryPassword,
                    confirmEmailLink,
                    cancellationToken));

            // Update roles
            await UpdateRoles(user.Id, request.Roles, cancellationToken);

            return Result.Success();
        }

        if (emailNotConfirmed)
        {
            // Scenario 3: Email not confirmed previously
            // Save user changes first
            var updateResult = await userRepository.UpdateAsync(user);
            if (updateResult.IsFailure)
                return updateResult;

            // Generate confirmation link for existing email
            var confirmEmailLink = $"/account/confirm-email?userId={user.Id}&email={Uri.EscapeDataString(user.Email!)}&code=EXISTING";

            // Send email asking user to confirm existing email
            logger.LogInformation(
                "Email confirmation request sent to {Email} for user {UserId} (email was not confirmed)",
                user.Email, request.Id);

            backgroundJobService.Enqueue<IEmailService>(emailService =>
                emailService.SendEmailConfirmationRequestAsync(user.Email!, user.FullName, confirmEmailLink, cancellationToken));

            // Update roles
            await UpdateRoles(user.Id, request.Roles, cancellationToken);

            return Result.Success();
        }

        // Scenario 2: Email not changed and already confirmed
        // Save changes without sending any email
        var saveResult = await userRepository.UpdateAsync(user);
        if (saveResult.IsFailure)
            return saveResult;

        await UpdateRoles(user.Id, request.Roles, cancellationToken);

        return Result.Success();
    }

    private async Task UpdateRoles(string userId, IList<string>? newRoles, CancellationToken cancellationToken)
    {
        // Get default roles that should always be kept
        var defaultRoles = await roleRepository.GetDefaultRolesAsync(cancellationToken);

        if (newRoles == null || !newRoles.Any())
        {
            // If no roles provided, ensure user has only default roles
            await roleRepository.ResetUserToDefaultRoleAsync(userId, cancellationToken);
            return;
        }

        // Combine requested roles with default roles (default roles are always kept)
        var rolesWithDefaults = newRoles.Union(defaultRoles).ToList();

        // Replace roles while keeping defaults
        await roleRepository.ReplaceUserRolesAsync(userId, rolesWithDefaults, cancellationToken);
    }

    private static string GenerateTemporaryPassword()
    {
        // Generate a temporary password that meets ValidationPatterns.Password requirements
        // Must have: 8+ chars, lowercase, uppercase, digit, special char
        var randomBytes = new byte[4];
        RandomNumberGenerator.Fill(randomBytes);
        var random = new Random(BitConverter.ToInt32(randomBytes, 0));

        const string lower = "abcdefghijklmnopqrstuvwxyz";
        const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string digits = "0123456789";
        const string special = "!@#$%^&*";

        // Ensure at least one of each required type
        var password = new char[12];
        password[0] = lower[random.Next(lower.Length)];
        password[1] = upper[random.Next(upper.Length)];
        password[2] = digits[random.Next(digits.Length)];
        password[3] = special[random.Next(special.Length)];

        // Fill the rest with random characters from all sets
        const string all = lower + upper + digits + special;
        for (int i = 4; i < password.Length; i++)
            password[i] = all[random.Next(all.Length)];

        // Shuffle
        for (int i = 0; i < password.Length; i++)
        {
            int j = random.Next(password.Length);
            (password[i], password[j]) = (password[j], password[i]);
        }

        return new string(password);
    }
}
