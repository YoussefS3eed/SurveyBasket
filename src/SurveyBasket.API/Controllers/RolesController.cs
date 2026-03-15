using SurveyBasket.Application.Features.Roles.Commands.CreateRole;
using SurveyBasket.Application.Features.Roles.Commands.ToggleRoleStatus;
using SurveyBasket.Application.Features.Roles.Commands.UpdateRole;
using SurveyBasket.Application.Features.Roles.Queries.GetAllRoles;
using SurveyBasket.Application.Features.Roles.Queries.GetRoleById;

namespace SurveyBasket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController(ISender sender) : ControllerBase
{
    [HttpGet("")]
    [HasPermission(Permissions.GetRoles)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDisabled, CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetAllRolesQuery(includeDisabled), cancellationToken))
            .ToActionResult(this);
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.GetRoles)]
    public async Task<IActionResult> Get([FromRoute] string id, CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetRoleByIdQuery(id), cancellationToken))
            .ToActionResult(this);
    }

    [HttpPost("")]
    [HasPermission(Permissions.AddRoles)]
    public async Task<IActionResult> Create([FromBody] CreateRoleCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.ToActionResult(this);

        return CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateRoles)]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateRoleCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command with { Id = id }, cancellationToken))
            .ToActionResult(this);
    }

    [HttpPut("{id}/toggle-status")]
    [HasPermission(Permissions.UpdateRoles)]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id, CancellationToken cancellationToken)
    {
        return (await sender.Send(new ToggleRoleStatusCommand(id), cancellationToken))
            .ToActionResult(this);
    }
}
