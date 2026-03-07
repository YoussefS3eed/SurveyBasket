using Microsoft.Extensions.Logging;
using SurveyBasket.Application.Interfaces;
using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Application.Authentication.Commands.Register;

public class RegisterCommandHandler(
    IUserRepository userRepository,
    IEmailService emailService,
    ILogger<RegisterCommandHandler> logger)
    : IRequestHandler<RegisterCommand, Result>
{
    public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.IsEmailExistsAsync(request.Email, cancellationToken))
            return Result.Failure(UserErrors.DuplicatedEmail);

        if (await userRepository.IsUsernameExistsAsync(request.Username, cancellationToken))
            return Result.Failure(UserErrors.DuplicatedUsername);

        var user = request.Adapt<ApplicationUser>();


        var result = await userRepository.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            var code = await userRepository.GenerateEmailConfirmationTokenAsync(user);
            code = code.ToBase64UrlEncoded();

            logger.LogInformation("Confirmation code for user {Email}: {Code}", request.Email, code);

            await emailService.SendConfirmationEmailAsync(user, code, cancellationToken);

            return Result.Success();
        }

        var error = result.Errors.First();
        return Result.Failure(new Error($"User.{error.Code}", error.Description, 400));
    }
}
