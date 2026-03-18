using SurveyBasket.Application.Common.Caching;
using SurveyBasket.Application.Common.Constants;
using SurveyBasket.Application.Features.Polls.Dtos;

namespace SurveyBasket.Application.Features.Polls.Queries.GetCurrentPollsV2;

public record GetCurrentPollsQueryV2 : ICachedQuery<Result<IEnumerable<PollResponseV2>>>
{
    public string CacheKey => CacheKeys.CurrentPolls;
    public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
    public TimeSpan? LocalCacheExpiration => TimeSpan.FromMinutes(1);
}