using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Users.Commands.ChangePassword;

public sealed class ChangePasswordCommandHandler(IUserRepository userRepository, ICurrentUserService currentUser)
    : IRequestHandler<ChangePasswordCommand, Result>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(currentUser.Id!, cancellationToken);

        var result = await userRepository.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

        return result;
    }
}
