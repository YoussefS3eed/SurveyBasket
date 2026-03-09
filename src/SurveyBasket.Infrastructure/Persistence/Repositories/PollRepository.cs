namespace SurveyBasket.Infrastructure.Persistence.Repositories;

internal sealed class PollRepository(ApplicationDbContext context) : IPollRepository
{
    public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken ct = default)
        => await context.Polls.AsNoTracking().ToListAsync(ct);

    public async Task<IEnumerable<Poll>> GetCurrentAsync(CancellationToken ct = default)
        => await context.Polls
            .Where(p => p.IsPublished
                && p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
                && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow))
            .AsNoTracking()
            .ToListAsync(ct);

    public async Task<Poll?> GetByIdAsync(int id, CancellationToken ct = default)
        => await context.Polls.FindAsync([id], ct);

    public async Task AddAsync(Poll poll, CancellationToken ct = default)
        => await context.Polls.AddAsync(poll, ct);

    public void Update(Poll poll)
        => context.Polls.Update(poll);

    public void Delete(Poll poll)
        => context.Polls.Remove(poll);

    public async Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        => await context.Polls.AnyAsync(p => p.Id == id, ct);

    public async Task<bool> ExistsByTitleExceptIdAsync(string title, int? excludeId, CancellationToken ct = default)
        => await context.Polls.AnyAsync(
            p => p.Title == title && p.Id != excludeId, ct);

    public async Task<bool> IsPollAvailableAsync(int pollId, CancellationToken ct = default)
        => await context.Polls.AnyAsync(
            p => p.Id == pollId
                && p.IsPublished
                && p.StartsAt <= DateOnly.FromDateTime(DateTime.UtcNow)
                && p.EndsAt >= DateOnly.FromDateTime(DateTime.UtcNow),
            ct);
}