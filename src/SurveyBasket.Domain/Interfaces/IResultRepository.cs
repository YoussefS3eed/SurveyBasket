using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Domain.Interfaces;

public interface IResultRepository
{
    Task<Poll?> GetPollWithVotesAsync(int pollId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Vote>> GetVotesByPollIdAsync(int pollId, CancellationToken cancellationToken = default);
    Task<IEnumerable<VoteAnswer>> GetVoteAnswersByPollIdAsync(int pollId, CancellationToken cancellationToken = default);
}