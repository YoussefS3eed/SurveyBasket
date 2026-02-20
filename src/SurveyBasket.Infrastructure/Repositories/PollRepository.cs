using SurveyBasket.Domain.Entities;
using SurveyBasket.Domain.Repositories;
using SurveyBasket.Infrastructure.Persistence;

namespace SurveyBasket.Infrastructure.Repositories;

internal class PollRepository(ApplicationDbContext context) : IPollRepository
{
    public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken)
        => await context.Polls.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Poll?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await context.Polls.FindAsync([id], cancellationToken);

    public async Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken)
    {
        await context.Polls.AddAsync(poll, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return poll;
    }

    public async Task UpdateAsync(Poll poll, CancellationToken cancellationToken)
    {
        context.Polls.Update(poll);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Poll poll, CancellationToken cancellationToken)
    {
        context.Polls.Remove(poll);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> TogglePublishAsync(int id, CancellationToken cancellationToken)
    {
        var poll = await GetByIdAsync(id, cancellationToken);
        if (poll is null) return false;

        poll.IsPublished = !poll.IsPublished;
        await context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
