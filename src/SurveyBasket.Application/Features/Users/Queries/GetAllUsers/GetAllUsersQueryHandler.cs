using SurveyBasket.Application.Features.Users.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Users.Queries.GetAllUsers;

internal sealed class GetAllUsersQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserResponse>>>
{
    public async Task<Result<IEnumerable<UserResponse>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetAllWithRolesAsync(cancellationToken);

        var userResponses = users.Select(u => new UserResponse(
            u.user.Id,
            u.user.UserName!,
            u.user.FirstName,
            u.user.LastName,
            u.user.Email!,
            u.user.IsDisabled,
            u.roles
        ));

        return Result.Success(userResponses);
    }
}
