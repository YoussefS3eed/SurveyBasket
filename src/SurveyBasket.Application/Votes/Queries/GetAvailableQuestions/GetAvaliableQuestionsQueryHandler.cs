using SurveyBasket.Application.Votes.Dtos;

namespace SurveyBasket.Application.Votes.Queries.GetAvailableQuestions;

public class GetAvailableQuestionsQueryHandler(
    IVoteRepository voteRepository,
    IQuestionRepository questionRepository,
    IPollRepository pollRepository)
    : IRequestHandler<GetAvailableQuestionsQuery, Result<IEnumerable<AvailableQuestionResponse>>>
{
    public async Task<Result<IEnumerable<AvailableQuestionResponse>>> Handle(
        GetAvailableQuestionsQuery request,
        CancellationToken cancellationToken)
    {
        var hasVoted = await voteRepository.HasVotedAsync(request.PollId, request.UserId, cancellationToken);
        if (hasVoted)
            return Result.Failure<IEnumerable<AvailableQuestionResponse>>(VoteErrors.DuplicatedVote);

        var pollAvailable = await pollRepository.IsPollAvailableAsync(request.PollId, cancellationToken);
        if (!pollAvailable)
            return Result.Failure<IEnumerable<AvailableQuestionResponse>>(PollErrors.PollNotFound);

        var questions = await questionRepository.GetActiveQuestionsByPollIdAsync(request.PollId, cancellationToken);
        if (!questions.Any())
            return Result.Failure<IEnumerable<AvailableQuestionResponse>>(PollErrors.PollNotFound);

        var response = questions.Adapt<IEnumerable<AvailableQuestionResponse>>();
        return Result.Success(response);
    }
}