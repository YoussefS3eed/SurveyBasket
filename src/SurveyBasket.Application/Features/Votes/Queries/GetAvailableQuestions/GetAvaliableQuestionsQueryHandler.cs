using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Application.Features.Votes.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Votes.Queries.GetAvailableQuestions;

public class GetAvailableQuestionsQueryHandler(
    IQuestionRepository questionRepository,
    IVoteRepository voteRepository,
    IPollRepository pollRepository,
    ICurrentUserService currentUser)
    : IRequestHandler<GetAvailableQuestionsQuery, Result<IEnumerable<AvailableQuestionResponse>>>
{
    public async Task<Result<IEnumerable<AvailableQuestionResponse>>> Handle(
        GetAvailableQuestionsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.Id!;

        var hasVoted = await voteRepository.HasVotedAsync(request.PollId, userId, cancellationToken);
        if (hasVoted)
            return VoteErrors.DuplicatedVote;

        var pollAvailable = await pollRepository.IsPollAvailableAsync(request.PollId, cancellationToken);
        if (!pollAvailable)
            return Result.Failure<IEnumerable<AvailableQuestionResponse>>(PollErrors.NotFound());

        var questions = await questionRepository.GetActiveQuestionsByPollIdAsync(request.PollId, cancellationToken);
        if (!questions.Any())
            return PollErrors.NotFound();

        var response = questions.Adapt<IEnumerable<AvailableQuestionResponse>>();
        return Result.Success(response);
    }
}