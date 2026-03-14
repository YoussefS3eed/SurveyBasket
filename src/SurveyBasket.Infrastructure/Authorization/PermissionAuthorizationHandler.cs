using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Application.Common.Contracts;

namespace SurveyBasket.Infrastructure.Authorization;

public sealed class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
            return Task.CompletedTask;

        var hasPermission = context.User.Claims
            .Any(c => c.Type == Permissions.Type && c.Value == requirement.Permission);

        if (hasPermission)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}