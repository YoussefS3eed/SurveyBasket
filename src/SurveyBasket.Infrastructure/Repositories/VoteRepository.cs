using SurveyBasket.Domain.Interfaces;
using SurveyBasket.Infrastructure.Persistence;

namespace SurveyBasket.Infrastructure.Repositories;

public class VoteRepository(ApplicationDbContext context) : IVoteRepository
{
    public async Task<bool> HasVotedAsync(int pollId, string userId, CancellationToken cancellationToken)
        => await context.Votes.AnyAsync(v => v.PollId == pollId && v.UserId == userId, cancellationToken);

    public async Task AddAsync(Vote vote, CancellationToken cancellationToken)
    {
        await context.Votes.AddAsync(vote, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}