using Microsoft.Extensions.Logging;
using SurveyBasket.Application.Common.Contracts;
using SurveyBasket.Application.Common.Extensions;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Authentication.Commands.ConfirmEmail;

internal sealed class ConfirmEmailCommandHandler(IUserRepository userRepository, ILogger<ConfirmEmailCommandHandler> logger)
    : IRequestHandler<ConfirmEmailCommand, Result>
{
    public async Task<Result> Handle(
        ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        string code;
        try
        {
            code = request.Code.FromBase64UrlEncoded();
        }
        catch (FormatException)
        {
            return UserErrors.InvalidCode;
        }

        if (await userRepository.GetByIdAsync(request.UserId, cancellationToken) is not { } user)
            return UserErrors.NotFound();

        if (user.EmailConfirmed)
            return UserErrors.DuplicatedConfirmation;

        var confirmResult = await userRepository.ConfirmEmailAsync(user, code);

        if (confirmResult.IsFailure)
            return confirmResult;

        var currentRoles = await userRepository.GetUserRolesAndPermissionsAsync(user, cancellationToken);
        if (!currentRoles.roles.Any())
        {
            await userRepository.AddToRoleAsync(user, DefaultRoles.Member);
            logger.LogInformation("Default role '{Role}' assigned to user {UserId} on email confirmation",
                DefaultRoles.Member, user.Id);
        }

        logger.LogInformation("Email confirmed for user {UserId}. User can now login.", user.Id);

        return Result.Success();
    }
}