namespace SurveyBasket.Domain.Interfaces.Repositories;

public interface IPollRepository
{
    Task<IEnumerable<Poll>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<Poll>> GetCurrentAsync(CancellationToken ct = default);
    Task<Poll?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(Poll poll, CancellationToken ct = default);
    void Update(Poll poll);
    void Delete(Poll poll);
    Task<bool> ExistsAsync(int id, CancellationToken ct = default);
    Task<bool> ExistsByTitleExceptIdAsync(string title, int? excludeId, CancellationToken ct = default);
    Task<bool> IsPollAvailableAsync(int pollId, CancellationToken ct = default);
}