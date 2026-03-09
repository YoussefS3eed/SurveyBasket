using SurveyBasket.Application.Features.Results.Dtos;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Results.Queries.GetVotesPerQuestion;

public class GetVotesPerQuestionQueryHandler(
    IResultRepository resultRepository,
    IPollRepository pollRepository)
    : IRequestHandler<GetVotesPerQuestionQuery, Result<IEnumerable<VotesPerQuestionResponse>>>
{
    public async Task<Result<IEnumerable<VotesPerQuestionResponse>>> Handle(
        GetVotesPerQuestionQuery request,
        CancellationToken cancellationToken)
    {
        var pollExists = await pollRepository.ExistsAsync(request.PollId, cancellationToken);
        if (!pollExists)
            return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollErrors.NotFound());

        var voteAnswers = await resultRepository.GetVoteAnswersByPollIdAsync(request.PollId, cancellationToken);

        var votesPerQuestion = voteAnswers
            .GroupBy(va => va.Question.Content)
            .Select(g => new VotesPerQuestionResponse(
                g.Key,
                g.GroupBy(va => va.Answer.Content)
                    .Select(ag => new VotesPerAnswerResponse(ag.Key, ag.Count()))
            ))
            .ToList();

        return Result.Success<IEnumerable<VotesPerQuestionResponse>>(votesPerQuestion);
    }
}