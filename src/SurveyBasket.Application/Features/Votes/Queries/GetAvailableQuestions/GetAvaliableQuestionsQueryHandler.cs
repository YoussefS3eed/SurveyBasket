using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Application.Features.Votes.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Votes.Queries.GetAvailableQuestions;

public class GetAvailableQuestionsQueryHandler(
    IQuestionRepository questionRepository,
    IVoteRepository voteRepository,
    IPollRepository pollRepository,
    ICurrentUserService currentUser)
    : IRequestHandler<GetAvailableQuestionsQuery, Result<IEnumerable<AvailableQuestionResponseDto>>>
{
    public async Task<Result<IEnumerable<AvailableQuestionResponseDto>>> Handle(
        GetAvailableQuestionsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = currentUser.Id!;

        var hasVoted = await voteRepository.HasVotedAsync(request.PollId, userId, cancellationToken);
        if (hasVoted)
            return VoteErrors.DuplicatedVote;

        var pollAvailable = await pollRepository.IsPollAvailableAsync(request.PollId, cancellationToken);
        if (!pollAvailable)
            return Result.Failure<IEnumerable<AvailableQuestionResponseDto>>(PollErrors.NotFound());

        var questions = await questionRepository.GetActiveQuestionsByPollIdAsync(request.PollId, cancellationToken);
        if (!questions.Any())
            return PollErrors.NotFound();

        var response = questions.Adapt<IEnumerable<AvailableQuestionResponseDto>>();
        return Result.Success(response);
    }
}