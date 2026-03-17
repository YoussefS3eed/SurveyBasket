using SurveyBasket.Application.Common.Caching;
using SurveyBasket.Application.Common.Constants;
using SurveyBasket.Application.Features.Polls.Dtos;

namespace SurveyBasket.Application.Features.Polls.Queries.GetCurrentPolls;

public record GetCurrentPollsQuery : ICachedQuery<Result<IEnumerable<PollResponse>>>
{
    public string CacheKey => CacheKeys.CurrentPolls;
    public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
    public TimeSpan? LocalCacheExpiration => TimeSpan.FromMinutes(1);
}