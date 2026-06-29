using SurveyBasket.Application.Features.Results.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Results.Queries.GetVotesPerQuestion;

public class GetVotesPerQuestionQueryHandler(
    IResultRepository resultRepository,
    IPollRepository pollRepository)
    : IRequestHandler<GetVotesPerQuestionQuery, Result<IEnumerable<VotesPerQuestionResponseDto>>>
{
    public async Task<Result<IEnumerable<VotesPerQuestionResponseDto>>> Handle(
        GetVotesPerQuestionQuery request,
        CancellationToken cancellationToken)
    {
        var pollExists = await pollRepository.ExistsAsync(request.PollId, cancellationToken);
        if (!pollExists)
            return Result.Failure<IEnumerable<VotesPerQuestionResponseDto>>(PollErrors.NotFound());

        var voteAnswers = await resultRepository.GetVoteAnswersByPollIdAsync(request.PollId, cancellationToken);

        var votesPerQuestion = voteAnswers
            .GroupBy(va => va.Question.Content)
            .Select(g => new VotesPerQuestionResponseDto(
                g.Key,
                g.GroupBy(va => va.Answer.Content)
                    .Select(ag => new VotesPerAnswerResponseDto(ag.Key, ag.Count()))
            ))
            .ToList();

        return Result.Success<IEnumerable<VotesPerQuestionResponseDto>>(votesPerQuestion);
    }
}