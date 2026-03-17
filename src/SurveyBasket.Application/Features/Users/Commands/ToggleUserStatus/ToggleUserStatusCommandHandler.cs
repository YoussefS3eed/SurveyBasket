using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Users.Commands.ToggleUserStatus;

internal sealed class ToggleUserStatusCommandHandler(IUserRepository userRepository)
    : IRequestHandler<ToggleUserStatusCommand, Result>
{
    public async Task<Result> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
    {
        return await userRepository.ToggleStatusAsync(request.Id, cancellationToken);
    }
}
