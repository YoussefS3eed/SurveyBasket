using SurveyBasket.Infrastructure.Persistence;

namespace SurveyBasket.Infrastructure.Repositories;

public class QuestionRepository(ApplicationDbContext context) : IQuestionRepository
{
    public async Task<IEnumerable<Question>> GetByPollIdAsync(int pollId, CancellationToken cancellationToken) =>
        await context.Questions
        .Where(q => q.PollId == pollId)
        .Include(q => q.Answers)
        .AsNoTracking()
        .ToListAsync(cancellationToken);

    //public async Task<IEnumerable<Question>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken = default) =>
    //    await context.Questions
    //        .Where(x => x.PollId == pollId && x.IsActive)
    //        .Include(x => x.Answers)
    //        .Select(q => new QuestionResponse(
    //            q.Id,
    //            q.Content
    //            q.
    //        ))
    //        .AsNoTracking()
    //        .ToListAsync(cancellationToken);


    public async Task<Question?> GetByIdAsync(int pollId, int id, bool includeAnswers = true, CancellationToken cancellationToken = default)
    {
        var query = context.Questions.AsQueryable();
        if (includeAnswers) query = query.Include(q => q.Answers);
        return await query.SingleOrDefaultAsync(q => q.PollId == pollId && q.Id == id, cancellationToken);
    }

    public async Task CreateAsync(Question question, CancellationToken cancellationToken)
    {
        await context.Questions.AddAsync(question, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Question question, CancellationToken cancellationToken)
    {
        context.Questions.Update(question);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsByContentExceptIdAsync(int pollId, string content, int? excludeId, CancellationToken cancellationToken)
    {
        var query = context.Questions.Where(q => q.PollId == pollId && q.Content == content);
        if (excludeId.HasValue) query = query.Where(q => q.Id != excludeId);
        return await query.AnyAsync(cancellationToken);
    }

    public async Task<List<int>> GetActiveQuestionIdsByPollIdAsync(int pollId, CancellationToken cancellationToken)
        => await context.Questions
            .Where(q => q.PollId == pollId && q.IsActive)
            .Select(q => q.Id)
            .ToListAsync(cancellationToken);

    // Also add a method to get full active questions with answers for the query handler
    public async Task<IEnumerable<Question>> GetActiveQuestionsByPollIdAsync(int pollId, CancellationToken cancellationToken)
        => await context.Questions
            .Include(q => q.Answers.Where(a => a.IsActive))
            .Where(q => q.PollId == pollId && q.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
}