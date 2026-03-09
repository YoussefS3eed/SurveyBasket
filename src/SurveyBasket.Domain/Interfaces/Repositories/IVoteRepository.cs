
namespace SurveyBasket.Domain.Interfaces.Repositories;

public interface IVoteRepository
{
    Task<bool> HasVotedAsync(int pollId, string userId, CancellationToken ct = default);
    Task AddAsync(Vote vote, CancellationToken ct = default);
}