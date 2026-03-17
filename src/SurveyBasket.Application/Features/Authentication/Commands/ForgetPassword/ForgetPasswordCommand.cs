namespace SurveyBasket.Application.Features.Authentication.Commands.ForgetPassword;

public sealed record ForgetPasswordCommand(string Email) : IRequest<Result>;
