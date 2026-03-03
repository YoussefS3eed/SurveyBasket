using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Application.Votes.Commands.CreateVote;

public class AddVoteCommandHandler(
    IVoteRepository voteRepository,
    IQuestionRepository questionRepository,
    IPollRepository pollRepository)
    : IRequestHandler<AddVoteCommand, Result>
{
    public async Task<Result> Handle(AddVoteCommand request, CancellationToken cancellationToken)
    {
        var hasVoted = await voteRepository.HasVotedAsync(request.PollId, request.UserId, cancellationToken);
        if (hasVoted)
            return Result.Failure(VoteErrors.DuplicatedVote);

        var pollAvailable = await pollRepository.IsPollAvailableAsync(request.PollId, cancellationToken);
        if (!pollAvailable)
            return Result.Failure(PollErrors.PollNotFound);

        var availableQuestionIds = await questionRepository.GetActiveQuestionIdsByPollIdAsync(request.PollId, cancellationToken);
        var requestedQuestionIds = request.Request.Answers.Select(a => a.QuestionId).ToList();

        if (!availableQuestionIds.SequenceEqual(requestedQuestionIds))
            return Result.Failure(VoteErrors.InvalidQuestions);

        var vote = new Vote
        {
            PollId = request.PollId,
            UserId = request.UserId,
            VoteAnswers = request.Request.Answers.Adapt<List<VoteAnswer>>()
        };

        await voteRepository.AddAsync(vote, cancellationToken);

        return Result.Success();
    }
}