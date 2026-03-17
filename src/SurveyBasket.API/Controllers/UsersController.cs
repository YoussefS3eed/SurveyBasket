using SurveyBasket.Application.Features.Users.Commands.CreateUser;
using SurveyBasket.Application.Features.Users.Commands.ResendUserConfirmationEmail;
using SurveyBasket.Application.Features.Users.Commands.ToggleUserStatus;
using SurveyBasket.Application.Features.Users.Commands.UnlockUser;
using SurveyBasket.Application.Features.Users.Commands.UpdateUser;
using SurveyBasket.Application.Features.Users.Commands.VerifyEmailChangeCode;
using SurveyBasket.Application.Features.Users.Queries.GetAllUsers;
using SurveyBasket.Application.Features.Users.Queries.GetUserById;

namespace SurveyBasket.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(ISender sender) : ControllerBase
{
    [HttpGet("")]
    [HasPermission(Permissions.GetUsers)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetAllUsersQuery(), cancellationToken))
            .ToActionResult(this);
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.GetUsers)]
    public async Task<IActionResult> Get([FromRoute] string id, CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetUserByIdQuery(id), cancellationToken))
            .ToActionResult(this);
    }

    [HttpPost("")]
    [HasPermission(Permissions.AddUsers)]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.ToActionResult(this);

        return CreatedAtAction(nameof(Get), new { result.Value.Id }, result.Value);
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateUsers)]
    public async Task<IActionResult> Update([FromRoute] string id, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command with { Id = id }, cancellationToken))
            .ToActionResult(this);
    }

    [HttpPut("{id}/toggle-status")]
    [HasPermission(Permissions.UpdateUsers)]
    public async Task<IActionResult> ToggleStatus([FromRoute] string id, CancellationToken cancellationToken)
    {
        return (await sender.Send(new ToggleUserStatusCommand(id), cancellationToken))
            .ToActionResult(this, 204);
    }

    [HttpPut("{id}/unlock")]
    [HasPermission(Permissions.UpdateUsers)]
    public async Task<IActionResult> Unlock([FromRoute] string id, CancellationToken cancellationToken)
    {
        return (await sender.Send(new UnlockUserCommand(id), cancellationToken))
            .ToActionResult(this, 204);
    }

    [HttpPost("resend-confirmation")]
    [HasPermission(Permissions.UpdateUsers)]
    public async Task<IActionResult> ResendConfirmation([FromBody] ResendUserConfirmationEmailCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command, cancellationToken))
            .ToActionResult(this, 200);
    }

    [HttpPost("verify-email-change-code")]
    public async Task<IActionResult> VerifyEmailChangeCode([FromBody] VerifyEmailChangeCodeCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command, cancellationToken))
            .ToActionResult(this, 200);
    }
}
