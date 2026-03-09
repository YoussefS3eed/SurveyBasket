namespace SurveyBasket.Infrastructure.Persistence.Repositories;

internal sealed class VoteRepository(ApplicationDbContext context) : IVoteRepository
{
    public async Task<bool> HasVotedAsync(int pollId, string userId, CancellationToken ct = default)
        => await context.Votes.AnyAsync(v => v.PollId == pollId && v.UserId == userId, ct);

    public async Task AddAsync(Vote vote, CancellationToken ct = default)
        => await context.Votes.AddAsync(vote, ct);
}