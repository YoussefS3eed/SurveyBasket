using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Users.Commands.UnlockUser;

internal sealed class UnlockUserCommandHandler(IUserRepository userRepository)
    : IRequestHandler<UnlockUserCommand, Result>
{
    public async Task<Result> Handle(UnlockUserCommand request, CancellationToken cancellationToken)
    {
        return await userRepository.UnlockAsync(request.Id, cancellationToken);
    }
}
