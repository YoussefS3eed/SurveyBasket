using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Domain.Interfaces;

public interface IQuestionRepository
{
    Task<IEnumerable<Question>> GetByPollIdAsync(int pollId, CancellationToken cancellationToken = default);
    //Task<IEnumerable<Question>> GetAvailableAsync(int pollId, string userId, CancellationToken cancellationToken = default);
    Task<Question?> GetByIdAsync(int pollId, int id, bool includeAnswers = true, CancellationToken cancellationToken = default);
    Task CreateAsync(Question question, CancellationToken cancellationToken = default);
    Task UpdateAsync(Question question, CancellationToken cancellationToken = default);
    Task<bool> ExistsByContentExceptIdAsync(int pollId, string content, int? excludeId = null, CancellationToken cancellationToken = default);
    Task<List<int>> GetActiveQuestionIdsByPollIdAsync(int pollId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Question>> GetActiveQuestionsByPollIdAsync(int pollId, CancellationToken cancellationToken = default);
}