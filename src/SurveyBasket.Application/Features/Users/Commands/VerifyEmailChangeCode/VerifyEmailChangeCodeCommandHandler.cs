using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Users.Commands.VerifyEmailChangeCode;

internal sealed class VerifyEmailChangeCodeCommandHandler(IUserRepository userRepository)
    : IRequestHandler<VerifyEmailChangeCodeCommand, Result>
{
    public async Task<Result> Handle(VerifyEmailChangeCodeCommand request, CancellationToken cancellationToken)
    {
        // 1. Verify the code
        var (isValid, newEmail) = await userRepository.VerifyEmailCodeAsync(request.UserId, request.Code, cancellationToken);

        if (!isValid)
            return Result.Failure(UserErrors.InvalidCode);

        if (string.IsNullOrEmpty(newEmail))
            return Result.Failure(UserErrors.InvalidCode);

        // 2. Find the user
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result.Failure(UserErrors.NotFound(request.UserId));

        // 3. Update email and mark as confirmed
        user.Email = newEmail;
        user.EmailConfirmed = true;

        var updateResult = await userRepository.UpdateAsync(user);
        if (updateResult.IsFailure)
            return updateResult;

        return Result.Success();
    }
}
