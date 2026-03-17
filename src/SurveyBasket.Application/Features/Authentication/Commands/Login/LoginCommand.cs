using SurveyBasket.Application.Features.Authentication.Dtos;

namespace SurveyBasket.Application.Features.Authentication.Commands.Login;

public record LoginCommand(string EmailOrUserName, string Password, string? VerificationCode = null)
    : IRequest<Result<AuthResponse>>;