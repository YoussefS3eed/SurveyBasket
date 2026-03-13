using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Users.Commands.UpdateProfile;

public sealed class UpdateProfileCommandHandler(IUserRepository userRepository, ICurrentUserService currentUser)
    : IRequestHandler<UpdateProfileCommand, Result>
{
    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        await userRepository.UpdateProfileAsync(currentUser.Id!, request.FirstName, request.LastName);

        return Result.Success();
    }
}
