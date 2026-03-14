using SurveyBasket.Application.Features.Polls.Commands.CreatePoll;
using SurveyBasket.Application.Features.Polls.Commands.DeletePoll;
using SurveyBasket.Application.Features.Polls.Commands.TogglePollPublish;
using SurveyBasket.Application.Features.Polls.Commands.UpdatePoll;
using SurveyBasket.Application.Features.Polls.Queries.GetAllPolls;
using SurveyBasket.Application.Features.Polls.Queries.GetCurrentPolls;
using SurveyBasket.Application.Features.Polls.Queries.GetPollById;

namespace SurveyBasket.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class PollsController(ISender sender) : ControllerBase
{
    [HttpGet("")]
    [HasPermission(Permissions.GetPolls)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetAllPollsQuery(), cancellationToken))
            .ToActionResult(this);
    }

    [HttpGet("current")]
    [Authorize(Roles = DefaultRoles.Member)]
    public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetCurrentPollsQuery(), cancellationToken))
            .ToActionResult(this);
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.GetPolls)]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetPollByIdQuery(id), cancellationToken))
            .ToActionResult(this);
    }

    [HttpPost("")]
    [HasPermission(Permissions.AddPolls)]
    public async Task<IActionResult> Create([FromBody] CreatePollCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.ToActionResult(this);

        return CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdatePolls)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePollCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command with { Id = id }, cancellationToken))
            .ToActionResult(this);
    }

    [HttpDelete("{id}")]
    [HasPermission(Permissions.DeletePolls)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return (await sender.Send(new DeletePollCommand(id), cancellationToken))
            .ToActionResult(this);
    }

    [HttpPut("{id}/togglePublish")]
    [HasPermission(Permissions.UpdatePolls)]
    public async Task<IActionResult> TogglePublish(int id, CancellationToken cancellationToken)
    {
        return (await sender.Send(new TogglePollPublishCommand(id), cancellationToken))
            .ToActionResult(this);
    }
}