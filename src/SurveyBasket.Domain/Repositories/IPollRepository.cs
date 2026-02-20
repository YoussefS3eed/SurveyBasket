using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Domain.Repositories;

public interface IPollRepository
{
    Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Poll?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken = default);
    Task UpdateAsync(Poll poll, CancellationToken cancellationToken = default);
    Task DeleteAsync(Poll poll, CancellationToken cancellationToken = default);
    Task<bool> TogglePublishAsync(int id, CancellationToken cancellationToken = default);
}
