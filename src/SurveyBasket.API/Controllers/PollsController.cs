using MediatR;
using SurveyBasket.Application.Polls.Commands.CreatePoll;
using SurveyBasket.Application.Polls.Commands.DeletePoll;
using SurveyBasket.Application.Polls.Commands.TogglePublishPoll;
using SurveyBasket.Application.Polls.Commands.UpdatePoll;
using SurveyBasket.Application.Polls.Dtos;
using SurveyBasket.Application.Polls.Queries.GetAllPolls;
using SurveyBasket.Application.Polls.Queries.GetPollById;

namespace SurveyBasket.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllPollsQuery();
        var result = await sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var query = new GetPollByIdQuery(id);
        var result = await sender.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost("")]
    public async Task<IActionResult> Create(PollRequestDto request, CancellationToken cancellationToken)
    {
        var command = new CreatePollCommand(request);
        var result = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, PollRequestDto request, CancellationToken cancellationToken)
    {
        var command = new UpdatePollCommand(id, request);
        await sender.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var command = new DeletePollCommand(id);
        await sender.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish(int id, CancellationToken cancellationToken)
    {
        var command = new TogglePublishCommand(id);
        await sender.Send(command, cancellationToken);
        return NoContent();
    }
}