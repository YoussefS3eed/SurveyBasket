using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Domain.Interfaces;

public interface IVoteRepository
{
    Task<bool> HasVotedAsync(int pollId, string userId, CancellationToken cancellationToken = default);
    Task AddAsync(Vote vote, CancellationToken cancellationToken = default);
}