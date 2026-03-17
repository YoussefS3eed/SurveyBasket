using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Roles.Commands.ToggleRoleStatus;

public class ToggleRoleStatusCommandHandler(IRoleRepository roleRepository)
    : IRequestHandler<ToggleRoleStatusCommand, Result>
{
    public async Task<Result> Handle(ToggleRoleStatusCommand request, CancellationToken cancellationToken)
    {
        var role = await roleRepository.GetByIdAsync(request.Id, cancellationToken);

        if (role is null)
            return Result.Failure(RoleErrors.RoleNotFound(request.Id));

        await roleRepository.ToggleStatusAsync(request.Id, cancellationToken);

        return Result.Success();
    }
}
