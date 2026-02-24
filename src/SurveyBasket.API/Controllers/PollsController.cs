using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SurveyBasket.API.Controllers.Base;
using SurveyBasket.Application.Polls.Commands.CreatePoll;
using SurveyBasket.Application.Polls.Commands.DeletePoll;
using SurveyBasket.Application.Polls.Commands.TogglePollPublish;
using SurveyBasket.Application.Polls.Commands.UpdatePoll;
using SurveyBasket.Application.Polls.Dtos;
using SurveyBasket.Application.Polls.Queries.GetAllPolls;
using SurveyBasket.Application.Polls.Queries.GetPollById;

namespace SurveyBasket.API.Controllers;


[Route("api/[controller]")]
[Authorize]
public class PollsController(ISender sender) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetAllPollsQuery();
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var query = new GetPollByIdQuery(id);
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreatePollRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<CreatePollCommand>();
        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return HandleResult(result);

        return CreatedAtAction(nameof(Get), new { id = result.Value.Id }, result.Value);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePollRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<UpdatePollCommand>() with { Id = id };
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var command = new DeletePollCommand(id);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id}/togglePublish")]
    public async Task<IActionResult> TogglePublish(int id, CancellationToken cancellationToken)
    {
        var command = new TogglePollPublishCommand(id);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }
}