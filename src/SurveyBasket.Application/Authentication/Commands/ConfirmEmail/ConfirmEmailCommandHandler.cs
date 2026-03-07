namespace SurveyBasket.Application.Authentication.Commands.ConfirmEmail;

public class ConfirmEmailCommandHandler(
    IUserRepository userRepository)
    : IRequestHandler<ConfirmEmailCommand, Result>
{
    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        if (await userRepository.GetByIdAsync(request.UserId) is not { } user)
            return Result.Failure(UserErrors.InvalidCode);

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        string code;
        try
        {
            code = request.Code.FromBase64UrlEncoded();
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }

        var result = await userRepository.ConfirmEmailAsync(user, code);

        if (result.Succeeded)
            return Result.Success();

        var error = result.Errors.First();
        return Result.Failure(new Error($"User.{error.Code}", error.Description, 400));
    }
}