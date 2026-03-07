namespace SurveyBasket.Application.Authentication.Commands.ResendConfirmationEmail;

public record ResendConfirmationEmailCommand(string Email) : IRequest<Result>;