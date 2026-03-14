using SurveyBasket.Application.Features.Votes.Commands.CreateVote;
using SurveyBasket.Application.Features.Votes.Dtos;
using SurveyBasket.Application.Features.Votes.Queries.GetAvailableQuestions;

namespace SurveyBasket.API.Controllers;

[ApiController]
[Route("api/polls/{pollId}/vote")]
[Authorize(Roles = DefaultRoles.Member)]
public class VotesController(ISender sender) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> Start(int pollId, CancellationToken cancellationToken)
    {
        return (await sender.Send(new GetAvailableQuestionsQuery(pollId), cancellationToken))
            .ToActionResult(this);
    }

    [HttpPost("")]
    public async Task<IActionResult> Vote(int pollId, [FromBody] VoteRequest request, CancellationToken cancellationToken)
    {
        return (await sender.Send(new AddVoteCommand(pollId, request.Answers), cancellationToken))
            .ToActionResult(this);
    }
}