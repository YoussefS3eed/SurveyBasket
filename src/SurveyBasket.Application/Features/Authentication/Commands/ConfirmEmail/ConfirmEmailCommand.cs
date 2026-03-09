using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Authentication.Commands.ConfirmEmail;

public record ConfirmEmailCommand(
    string UserId,
    string Code
) : IRequest<Result>;