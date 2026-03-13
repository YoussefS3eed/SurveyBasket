using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Domain.Common.Dtos;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Users.Queries.GetProfile;

public sealed class GetProfileQueryHandler(IUserRepository userRepository, ICurrentUserService currentUser)
    : IRequestHandler<GetProfileQuery, Result<UserProfileDto>>
{
    public async Task<Result<UserProfileDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        return Result.Success(await userRepository.GetUserProfileByIdAsync(currentUser.Id!, cancellationToken));
    }
}
