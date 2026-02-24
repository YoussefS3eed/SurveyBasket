using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SurveyBasket.API.Controllers.Base;
using SurveyBasket.Application.Questions.Commands.CreateQuestion;
using SurveyBasket.Application.Questions.Commands.ToggleQuestionStatus;
using SurveyBasket.Application.Questions.Commands.UpdateQuestion;
using SurveyBasket.Application.Questions.Dtos;
using SurveyBasket.Application.Questions.Queries.GetQuestionById;
using SurveyBasket.Application.Questions.Queries.GetQuestionsByPollId;

namespace SurveyBasket.API.Controllers;

[Route("api/polls/{pollId}/[controller]")]
[Authorize]
public class QuestionsController(ISender sender) : ApiController
{
    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var query = new GetQuestionsByPollIdQuery(pollId);
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var query = new GetQuestionByIdQuery(pollId, id);
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromRoute] int pollId, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<CreateQuestionCommand>() with { PollId = pollId };
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int id, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var command = request.Adapt<UpdateQuestionCommand>() with { PollId = pollId, Id = id };
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }

    [HttpPut("{id}/toggleStatus")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var command = new ToggleQuestionStatusCommand(pollId, id);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }
}
