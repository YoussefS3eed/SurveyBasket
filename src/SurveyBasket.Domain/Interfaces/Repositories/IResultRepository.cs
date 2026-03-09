namespace SurveyBasket.Domain.Interfaces.Repositories;

public interface IResultRepository
{
    Task<Poll?> GetPollWithVotesAsync(int pollId, CancellationToken ct = default);
    Task<IEnumerable<Vote>> GetVotesByPollIdAsync(int pollId, CancellationToken ct = default);
    Task<IEnumerable<VoteAnswer>> GetVoteAnswersByPollIdAsync(int pollId, CancellationToken ct = default);
}