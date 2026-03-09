namespace SurveyBasket.Domain.Interfaces;

public interface IUnitOfWork
{
    /// <summary>
    /// Persist all tracked changes in a single atomic commit.
    /// Use this for single-aggregate or simple multi-step operations.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Wrap the given work inside an explicit database transaction.
    /// Use this when multiple SaveChanges calls must either all succeed or all roll back.
    /// The transaction is committed automatically on success and rolled back on exception.
    /// </summary>
    Task ExecuteInTransactionAsync(
        Func<CancellationToken, Task> work,
        CancellationToken cancellationToken = default);
}