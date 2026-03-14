using SurveyBasket.Application.Features.Results.Queries.GetPollVotes;
using SurveyBasket.Application.Features.Results.Queries.GetVotesPerDay;
using SurveyBasket.Application.Features.Results.Queries.GetVotesPerQuestion;

namespace SurveyBasket.API.Controllers;

[ApiController]
[Route("api/polls/{pollId}/[controller]")]
[HasPermission(Permissions.Results)]
public class ResultsController(ISender sender) : ControllerBase
{
    [HttpGet("raw-data")]
    public async Task<IActionResult> PollVotes(int pollId, CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetPollVotesQuery(pollId), cancellationToken))
            .ToActionResult(this);
    }

    [HttpGet("votes-per-day")]
    public async Task<IActionResult> VotesPerDay(int pollId, CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetVotesPerDayQuery(pollId), cancellationToken))
            .ToActionResult(this);
    }

    [HttpGet("votes-per-question")]
    public async Task<IActionResult> VotesPerQuestion(int pollId, CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetVotesPerQuestionQuery(pollId), cancellationToken))
            .ToActionResult(this);
    }
}