using SurveyBasket.Application.Features.Authentication.Dtos;
using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Authentication.Commands.Login;

public record LoginCommand(string EmailOrUserName, string Password)
    : IRequest<Result<AuthResponse>>;