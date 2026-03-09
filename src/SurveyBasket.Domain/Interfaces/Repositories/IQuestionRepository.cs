
namespace SurveyBasket.Domain.Interfaces.Repositories;

public interface IQuestionRepository
{
    Task<IEnumerable<Question>> GetByPollIdAsync(int pollId, CancellationToken ct = default);
    //Task<IEnumerable<Question>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken = default);
    Task<Question?> GetByIdAsync(int pollId, int id, bool includeAnswers = true, CancellationToken ct = default);
    Task AddAsync(Question question, CancellationToken ct = default);
    void Update(Question question);
    Task<bool> ExistsByContentExceptIdAsync(int pollId, string content, int? excludeId = null, CancellationToken ct = default);
    Task<List<int>> GetActiveQuestionIdsByPollIdAsync(int pollId, CancellationToken ct = default);
    Task<IEnumerable<Question>> GetActiveQuestionsByPollIdAsync(int pollId, CancellationToken ct = default);
}