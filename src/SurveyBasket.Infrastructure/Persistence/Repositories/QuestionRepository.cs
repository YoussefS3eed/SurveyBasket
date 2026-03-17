namespace SurveyBasket.Infrastructure.Persistence.Repositories;

internal sealed class QuestionRepository(ApplicationDbContext context) : IQuestionRepository
{
    public Task<IQueryable<Question>> GetByPollIdAsync(int pollId, CancellationToken ct = default)
        => Task.FromResult(context.Questions
            .Where(q => q.PollId == pollId)
            .Include(q => q.Answers)
            .AsNoTracking()
            .AsQueryable());

    public async Task<Question?> GetByIdAsync(int pollId, int id, bool includeAnswers = true, CancellationToken ct = default)
    {
        var query = context.Questions.Where(q => q.PollId == pollId && q.Id == id);

        if (includeAnswers)
            query = query.Include(q => q.Answers);

        return await query.FirstOrDefaultAsync(ct);
    }

    public async Task AddAsync(Question question, CancellationToken ct = default)
        => await context.Questions.AddAsync(question, ct);
    public void Update(Question question)
        => context.Questions.Update(question);

    public async Task<bool> ExistsByContentExceptIdAsync(
        int pollId, string content, int? excludeId = null, CancellationToken ct = default)
        => await context.Questions.AnyAsync(
            q => q.PollId == pollId
                && q.Content == content
                && q.Id != excludeId,
            ct);

    public async Task<List<int>> GetActiveQuestionIdsByPollIdAsync(int pollId, CancellationToken ct = default)
        => await context.Questions
            .Where(q => q.PollId == pollId && q.IsActive)
            .Select(q => q.Id)
            .OrderBy(id => id)
            .ToListAsync(ct);

    public async Task<IEnumerable<Question>> GetActiveQuestionsByPollIdAsync(int pollId, CancellationToken ct = default)
        => await context.Questions
            .Where(q => q.PollId == pollId && q.IsActive)
            .Include(q => q.Answers.Where(a => a.IsActive))
            .AsNoTracking()
            .ToListAsync(ct);
}