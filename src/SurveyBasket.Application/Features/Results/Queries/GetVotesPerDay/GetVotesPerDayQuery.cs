using SurveyBasket.Application.Common.Caching;
using SurveyBasket.Application.Common.Constants;
using SurveyBasket.Application.Features.Results.Dtos;

namespace SurveyBasket.Application.Features.Results.Queries.GetVotesPerDay;

public record GetVotesPerDayQuery(int PollId) : ICachedQuery<Result<IEnumerable<VotesPerDayResponse>>>
{
    public string CacheKey => CacheKeys.VotesPerDay(PollId);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(20);
    public TimeSpan? LocalCacheExpiration => TimeSpan.FromMinutes(5);
}