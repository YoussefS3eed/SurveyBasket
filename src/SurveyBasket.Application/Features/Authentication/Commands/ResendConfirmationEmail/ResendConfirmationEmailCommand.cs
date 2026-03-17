namespace SurveyBasket.Application.Features.Authentication.Commands.ResendConfirmationEmail;

public record ResendConfirmationEmailCommand(string Email) : IRequest<Result>;