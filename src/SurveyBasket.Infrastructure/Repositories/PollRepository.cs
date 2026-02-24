using SurveyBasket.Domain.Interfaces;
using SurveyBasket.Infrastructure.Persistence;

namespace SurveyBasket.Infrastructure.Repositories;

internal class PollRepository(ApplicationDbContext context) : IPollRepository
{
    public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.Polls.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<Poll?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.Polls.FindAsync([id], cancellationToken);

    public async Task<Poll> CreateAsync(Poll poll, CancellationToken cancellationToken = default)
    {
        await context.Polls.AddAsync(poll, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return poll;
    }

    public async Task UpdateAsync(Poll poll, CancellationToken cancellationToken = default)
    {
        context.Polls.Update(poll);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Poll poll, CancellationToken cancellationToken = default)
    {
        context.Polls.Remove(poll);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
        => await context.Polls.AnyAsync(x => x.Id == id, cancellationToken);

    public async Task<bool> ExistsByTitleExceptIdAsync(string title, int? excludeId, CancellationToken cancellationToken)
    {
        var query = context.Polls.Where(p => p.Title == title);
        if (excludeId.HasValue) query = query.Where(p => p.Id != excludeId);
        return await query.AnyAsync(cancellationToken);
    }

}