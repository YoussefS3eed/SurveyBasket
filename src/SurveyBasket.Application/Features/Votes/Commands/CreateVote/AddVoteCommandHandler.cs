using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Domain.Entities;
using SurveyBasket.Domain.Interfaces;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Votes.Commands.CreateVote;

public sealed class AddVoteCommandHandler(
    IVoteRepository voteRepository,
    IQuestionRepository questionRepository,
    IPollRepository pollRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser)
    : IRequestHandler<AddVoteCommand, Result>
{
    public async Task<Result> Handle(
        AddVoteCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.Id!;

        // --- Guards (read-only, no transaction needed yet) ---

        var hasVoted = await voteRepository.HasVotedAsync(request.PollId, userId, cancellationToken);

        if (hasVoted)
            return Result.Failure(VoteErrors.DuplicatedVote);

        var pollAvailable = await pollRepository.IsPollAvailableAsync(
            request.PollId, cancellationToken);

        if (!pollAvailable)
            return Result.Failure(PollErrors.NotFound(request.PollId));

        var availableIds = await questionRepository
            .GetActiveQuestionIdsByPollIdAsync(request.PollId, cancellationToken);

        var submittedIds = request.Answers
            .Select(a => a.QuestionId)
            .OrderBy(id => id)
            .ToList();

        if (!availableIds.OrderBy(id => id).SequenceEqual(submittedIds))
            return Result.Failure(VoteErrors.InvalidQuestions);

        // --- Write inside a transaction ---
        await unitOfWork.ExecuteInTransactionAsync(async ct =>
        {
            var vote = new Vote
            {
                PollId = request.PollId,
                UserId = userId,
                VoteAnswers = request.Answers.Adapt<List<VoteAnswer>>()
            };

            await voteRepository.AddAsync(vote, ct);
            await unitOfWork.SaveChangesAsync(ct);
        }, cancellationToken);

        return Result.Success();
    }
}