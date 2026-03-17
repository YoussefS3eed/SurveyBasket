using SurveyBasket.Application.Features.Users.Dtos;

namespace SurveyBasket.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(string Id)
    : IRequest<Result<UserResponse>>;
