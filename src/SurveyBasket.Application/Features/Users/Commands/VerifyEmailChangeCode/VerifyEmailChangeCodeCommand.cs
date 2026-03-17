namespace SurveyBasket.Application.Features.Users.Commands.VerifyEmailChangeCode;

public record VerifyEmailChangeCodeCommand(
    string UserId,
    string Code
) : IRequest<Result>;
