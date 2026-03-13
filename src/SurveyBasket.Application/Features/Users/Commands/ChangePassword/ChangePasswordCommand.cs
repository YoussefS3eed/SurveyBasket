using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Users.Commands.ChangePassword;

public sealed record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword
) : IRequest<Result>;
