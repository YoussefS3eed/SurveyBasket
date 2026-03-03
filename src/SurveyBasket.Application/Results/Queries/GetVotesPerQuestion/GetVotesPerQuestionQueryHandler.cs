using SurveyBasket.Contracts.Results;

namespace SurveyBasket.Application.Results.Queries.GetVotesPerQuestion;

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
            return Result.Failure<IEnumerable<VotesPerQuestionResponse>>(PollErrors.PollNotFound);

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