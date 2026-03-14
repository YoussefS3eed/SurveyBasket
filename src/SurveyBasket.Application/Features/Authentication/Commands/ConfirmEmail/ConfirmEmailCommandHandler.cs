using SurveyBasket.Application.Common.Contracts;
using SurveyBasket.Application.Common.Extensions;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Authentication.Commands.ConfirmEmail;

internal sealed class ConfirmEmailCommandHandler(IUserRepository userRepository)
    : IRequestHandler<ConfirmEmailCommand, Result>
{
    public async Task<Result> Handle(
        ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return UserErrors.InvalidCode;

        if (user.EmailConfirmed)
            return UserErrors.DuplicatedConfirmation;

        string code;
        try
        {
            code = request.Code.FromBase64UrlEncoded();
        }
        catch (FormatException)
        {
            return UserErrors.InvalidCode;
        }

        var result = await userRepository.ConfirmEmailAsync(user, code);

        if (result.IsFailure)
            return result;

        // Assign Member role after successful email confirmation
        await userRepository.AddToRoleAsync(user, DefaultRoles.Member);

        return result;
    }
}