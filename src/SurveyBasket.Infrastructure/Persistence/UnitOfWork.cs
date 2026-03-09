using Microsoft.EntityFrameworkCore.Storage;
using SurveyBasket.Domain.Interfaces;

namespace SurveyBasket.Infrastructure.Persistence;

internal sealed class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);

    public async Task ExecuteInTransactionAsync(
        Func<CancellationToken, Task> work,
        CancellationToken cancellationToken = default)
    {
        IDbContextTransaction? transaction = null;

        // Reuse an existing transaction if one is already active (nested calls)
        if (context.Database.CurrentTransaction is null)
        {
            transaction = await context.Database.BeginTransactionAsync(cancellationToken);
        }

        try
        {
            await work(cancellationToken);

            if (transaction is not null)
                await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            if (transaction is not null)
                await transaction.RollbackAsync(cancellationToken);

            throw;
        }
        finally
        {
            if (transaction is not null)
                await transaction.DisposeAsync();
        }
    }
}