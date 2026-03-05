using SurveyBasket.Application.Abstractions.Caching;
using SurveyBasket.Contracts.Results;

namespace SurveyBasket.Application.Results.Queries.GetPollVotes;

public record GetPollVotesQuery(int PollId)
    : ICachedQuery<Result<PollVotesResponse>>
{
    public string CacheKey => ConstCacheKeys.AllPolls;   // unique per poll
    public TimeSpan? Expiration => TimeSpan.FromMinutes(30);
    public TimeSpan? LocalCacheExpiration => TimeSpan.FromMinutes(5);
}