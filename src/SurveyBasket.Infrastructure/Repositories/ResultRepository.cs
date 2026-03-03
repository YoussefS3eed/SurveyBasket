using SurveyBasket.Infrastructure.Persistence;

namespace SurveyBasket.Infrastructure.Repositories;

public class ResultRepository(ApplicationDbContext context) : IResultRepository
{
    public async Task<Poll?> GetPollWithVotesAsync(int pollId, CancellationToken cancellationToken) =>
        await context.Polls
            .Include(p => p.Votes)
                .ThenInclude(v => v.User)
            .Include(p => p.Votes)
                .ThenInclude(v => v.VoteAnswers)
                    .ThenInclude(va => va.Question)
            .Include(p => p.Votes)
                .ThenInclude(v => v.VoteAnswers)
                    .ThenInclude(va => va.Answer)
            .FirstOrDefaultAsync(p => p.Id == pollId, cancellationToken);

    public async Task<IEnumerable<Vote>> GetVotesByPollIdAsync(int pollId, CancellationToken cancellationToken) =>
        await context.Votes
            .Where(v => v.PollId == pollId)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<VoteAnswer>> GetVoteAnswersByPollIdAsync(int pollId, CancellationToken cancellationToken) =>
        await context.VoteAnswers
            .Where(va => va.Vote.PollId == pollId)
            .Include(va => va.Question)
            .Include(va => va.Answer)
            .ToListAsync(cancellationToken);

}