using SurveyBasket.Application.Votes.Commands.CreateVote;
using SurveyBasket.Application.Votes.Dtos;
using SurveyBasket.Application.Votes.Queries.GetAvailableQuestions;
using System.Security.Claims;

namespace SurveyBasket.API.Controllers;

[Route("api/polls/{pollId}/vote")]
[Authorize]
public class VotesController(ISender sender) : ApiController
{
    [HttpGet("")]
    public async Task<IActionResult> Start(int pollId, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var query = new GetAvailableQuestionsQuery(pollId, userId!);
        var result = await sender.Send(query, cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("")]
    public async Task<IActionResult> Vote(int pollId, [FromBody] VoteRequest request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var command = new AddVoteCommand(pollId, userId!, request);
        var result = await sender.Send(command, cancellationToken);
        return HandleResult(result);
    }
}