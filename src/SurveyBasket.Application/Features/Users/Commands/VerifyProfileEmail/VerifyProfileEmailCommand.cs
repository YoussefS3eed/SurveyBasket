using SurveyBasket.Application.Features.Authentication.Dtos;

namespace SurveyBasket.Application.Features.Users.Commands.VerifyProfileEmail;

public sealed record VerifyProfileEmailCommand(
    string Code
) : IRequest<Result<AuthResponse>>;
