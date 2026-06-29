using SurveyBasket.Application.Features.Users.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Users.Queries.GetUserById;

internal sealed class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IRequestHandler<GetUserByIdQuery, Result<UserResponseDto>>
{
    public async Task<Result<UserResponseDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        if (await userRepository.GetByIdAsync(request.Id, cancellationToken) is not { } user)
            return Result.Failure<UserResponseDto>(UserErrors.NotFound(request.Id));

        var roles = await userRepository.GetRolesAsync(user);

        var response = new UserResponseDto(
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
