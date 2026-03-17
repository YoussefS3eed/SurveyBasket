using SurveyBasket.Application.Features.Users.Dtos;

namespace SurveyBasket.Application.Features.Users.Queries.GetAllUsers;

public record GetAllUsersQuery()
    : IRequest<Result<IEnumerable<UserResponse>>>;
