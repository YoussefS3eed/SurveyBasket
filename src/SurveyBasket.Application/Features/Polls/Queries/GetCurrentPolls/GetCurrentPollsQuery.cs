using SurveyBasket.Application.Common.Caching;
using SurveyBasket.Application.Common.Constants;
using SurveyBasket.Application.Features.Polls.Dtos;
using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Polls.Queries.GetCurrentPolls;

public record GetCurrentPollsQuery : ICachedQuery<Result<IEnumerable<PollDto>>>
{
    public string CacheKey => CacheKeys.CurrentPolls;
    public TimeSpan? Expiration => TimeSpan.FromMinutes(5);
    public TimeSpan? LocalCacheExpiration => TimeSpan.FromMinutes(1);
}