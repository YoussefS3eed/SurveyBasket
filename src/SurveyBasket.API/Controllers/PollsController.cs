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
[Authorize]
public class PollsController(ISender sender) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetAllPollsQuery(), cancellationToken))
            .ToActionResult(this);
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrent(CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetCurrentPollsQuery(), cancellationToken))
            .ToActionResult(this);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetPollByIdQuery(id), cancellationToken))
            .ToActionResult(this);
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreatePollCommand command, CancellationToken cancellationToken)
    {
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return result.ToActionResult(this);

        return CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePollCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command with { Id = id }, cancellationToken))
            .ToActionResult(this);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return (await sender.Send(new DeletePollCommand(id), cancellationToken))
            .ToActionResult(this);
    }

    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish(int id, CancellationToken cancellationToken)
    {
        return (await sender.Send(new TogglePollPublishCommand(id), cancellationToken))
            .ToActionResult(this);
    }
}