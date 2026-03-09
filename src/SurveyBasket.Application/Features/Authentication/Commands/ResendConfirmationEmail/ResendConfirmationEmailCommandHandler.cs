using Microsoft.Extensions.Logging;
using SurveyBasket.Application.Common.Extensions;
using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Authentication.Commands.ResendConfirmationEmail;

internal sealed class ResendConfirmationEmailCommandHandler(
    IUserRepository userRepository,
    IBackgroundJobService backgroundJobService,
    IApplicationUrlService urlService,
    ILogger<ResendConfirmationEmailCommandHandler> logger)
    : IRequestHandler<ResendConfirmationEmailCommand, Result>
{
    public async Task<Result> Handle(
        ResendConfirmationEmailCommand request, CancellationToken cancellationToken)
    {
        // Silently succeed if email not found — prevents user enumeration
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is null)
            return Result.Success();

        var isConfirmed = await userRepository.IsEmailConfirmedAsync(user);
        if (isConfirmed)
            return UserErrors.DuplicatedConfirmation;

        var code = await userRepository.GenerateEmailConfirmationTokenAsync(user);
        code = code.ToBase64UrlEncoded();

        logger.LogInformation(
            "Resent confirmation for {Email}: {Code}", request.Email, code);

        var origin = urlService.GetOrigin();
        var confirmationLink =
            $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}";

        backgroundJobService.Enqueue<IEmailService>(emailService => emailService.SendConfirmationEmailAsync(user.Email!, user.FullName, confirmationLink, cancellationToken));


        return Result.Success();
    }
}