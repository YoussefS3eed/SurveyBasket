using SurveyBasket.Application.Common.Models;
using SurveyBasket.Application.Features.Questions.Commands.CreateQuestion;
using SurveyBasket.Application.Features.Questions.Commands.ToggleQuestionStatus;
using SurveyBasket.Application.Features.Questions.Commands.UpdateQuestion;
using SurveyBasket.Application.Features.Questions.Queries.GetQuestionById;
using SurveyBasket.Application.Features.Questions.Queries.GetQuestionsByPollId;

namespace SurveyBasket.API.Controllers;

[ApiController]
[Route("api/polls/{pollId}/[controller]")]
[Authorize]
public class QuestionsController(ISender sender) : ControllerBase
{
    [HttpGet("")]
    [HasPermission(Permissions.GetQuestions)]
    public async Task<IActionResult> GetAll([FromRoute] int pollId, [FromQuery] RequestFilters filters, CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetQuestionsByPollIdQuery(pollId, filters), cancellationToken))
            .ToActionResult(this);
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.GetQuestions)]
    public async Task<IActionResult> Get([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetQuestionByIdQuery(pollId, id), cancellationToken))
            .ToActionResult(this);
    }

    [HttpPost("")]
    [HasPermission(Permissions.AddQuestions)]
    public async Task<IActionResult> Create([FromRoute] int pollId, [FromBody] CreateQuestionCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command with { PollId = pollId }, cancellationToken))
            .ToActionResult(this);
    }

    [HttpPut("{id}")]
    [HasPermission(Permissions.UpdateQuestions)]
    public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int id, [FromBody] UpdateQuestionCommand command, CancellationToken cancellationToken)
    {
        return (await sender.Send(command with { PollId = pollId, Id = id }, cancellationToken))
            .ToActionResult(this);
    }

    [HttpPut("{id}/toggleStatus")]
    [HasPermission(Permissions.UpdateQuestions)]
    public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        return (await sender.Send(new ToggleQuestionStatusCommand(pollId, id), cancellationToken))
            .ToActionResult(this);
    }
}
