using Microsoft.Extensions.Logging;
using SurveyBasket.Application.Common.Extensions;
using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Entities;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Authentication.Commands.Register;

internal sealed class RegisterCommandHandler(
    IUserRepository userRepository,
    IEmailService emailService,
    IApplicationUrlService urlService,    // ✅ isolates HttpContext
    ILogger<RegisterCommandHandler> logger)
    : IRequestHandler<RegisterCommand, Result>
{
    public async Task<Result> Handle(
        RegisterCommand request, CancellationToken cancellationToken)
    {
        // 1. Uniqueness guards
        if (await userRepository.IsEmailExistsAsync(request.Email, cancellationToken))
            return UserErrors.DuplicatedEmail();

        if (await userRepository.IsUsernameExistsAsync(request.Username, cancellationToken))
            return UserErrors.DuplicatedUsername();

        // 2. Create entity using factory method
        var user = ApplicationUser.Create(
            request.FirstName,
            request.LastName,
            request.Username,
            request.Email);

        // 3. Persist — wraps IdentityResult internally
        var createResult = await userRepository.CreateAsync(user, request.Password);

        if (createResult.IsFailure)
            return createResult.Error;

        // 4. Generate confirmation token
        var code = await userRepository.GenerateEmailConfirmationTokenAsync(user);
        code = code.ToBase64UrlEncoded();

        logger.LogInformation(
            "Confirmation code generated for {Email}: {Code}", request.Email, code);

        // 5. Build confirmation URL
        var origin = urlService.GetOrigin();
        var confirmationLink =
            $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}";

        // 6. Send email with primitives only — ✅ no ApplicationUser passed
        await emailService.SendConfirmationEmailAsync(
            user.Email!,
            user.FullName,      // ✅ computed property on ApplicationUser
            confirmationLink,
            cancellationToken);

        return Result.Success();
    }
}
