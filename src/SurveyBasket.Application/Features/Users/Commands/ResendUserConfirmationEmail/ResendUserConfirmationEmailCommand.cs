namespace SurveyBasket.Application.Features.Users.Commands.ResendUserConfirmationEmail;

public record ResendUserConfirmationEmailCommand(string Email) : IRequest<Result>;
