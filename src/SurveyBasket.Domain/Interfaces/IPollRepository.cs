using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Domain.Interfaces;

public interface IPollRepository
{
    Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Poll?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Poll> CreateAsync(Poll poll, CancellationToken cancellationToken = default);
    Task UpdateAsync(Poll poll, CancellationToken cancellationToken = default);
    Task DeleteAsync(Poll poll, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken);
    Task<bool> ExistsByTitleExceptIdAsync(string title, int? excludeId, CancellationToken cancellationToken);
}