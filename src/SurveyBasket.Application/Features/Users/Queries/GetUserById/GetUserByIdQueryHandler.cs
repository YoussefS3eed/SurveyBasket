using Microsoft.AspNetCore.Identity;
using SurveyBasket.Application.Features.Users.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Users.Queries.GetUserById;

internal sealed class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserByIdQuery, Result<UserResponse>>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        if (await userRepository.GetByIdAsync(request.Id, cancellationToken) is not { } user)
            return Result.Failure<UserResponse>(UserErrors.NotFound(request.Id));

        var roles = await userRepository.GetRolesAsync(user);

        var response = new UserResponse(
            user.Id,
            user.UserName!,
            user.FirstName,
            user.LastName,
            user.Email!,
            user.IsDisabled,
            roles);

        return Result.Success(response);
    }
}
