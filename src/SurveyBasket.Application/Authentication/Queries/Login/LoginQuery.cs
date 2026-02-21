using SurveyBasket.Application.Authentication.Dtos;

namespace SurveyBasket.Application.Authentication.Queries.Login;

public record LoginQuery(string Email, string Password)
    : IRequest<Result<AuthResponseDto>>;