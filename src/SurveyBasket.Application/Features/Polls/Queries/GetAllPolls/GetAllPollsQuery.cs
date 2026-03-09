using SurveyBasket.Application.Common.Caching;
using SurveyBasket.Application.Common.Constants;
using SurveyBasket.Application.Features.Polls.Dtos;
using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Polls.Queries.GetAllPolls;

public record GetAllPollsQuery : ICachedQuery<Result<IEnumerable<PollResponse>>>
{
    public string CacheKey => CacheKeys.AllPolls;
    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    public TimeSpan? LocalCacheExpiration => TimeSpan.FromMinutes(2);
}