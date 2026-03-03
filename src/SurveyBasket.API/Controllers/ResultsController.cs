using SurveyBasket.Application.Results.Queries.GetPollVotes;
using SurveyBasket.Application.Results.Queries.GetVotesPerDay;
using SurveyBasket.Application.Results.Queries.GetVotesPerQuestion;

namespace SurveyBasket.API.Controllers;

[Route("api/polls/{pollId}/[controller]")]
[Authorize]
public class ResultsController(ISender sender) : ApiController
{
    [HttpGet("row-data")]
    public async Task<IActionResult> PollVotes(int pollId, CancellationToken cancellationToken)
    {
        var query = new GetPollVotesQuery(pollId);
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("votes-per-day")]
    public async Task<IActionResult> VotesPerDay(int pollId, CancellationToken cancellationToken)
    {
        var query = new GetVotesPerDayQuery(pollId);
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("votes-per-question")]
    public async Task<IActionResult> VotesPerQuestion(int pollId, CancellationToken cancellationToken)
    {
        var query = new GetVotesPerQuestionQuery(pollId);
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result);
    }
}