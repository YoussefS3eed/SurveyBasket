using SurveyBasket.Application.Common.Caching;
using SurveyBasket.Application.Common.Constants;
using SurveyBasket.Application.Features.Polls.Dtos;

namespace SurveyBasket.Application.Features.Polls.Queries.GetPollById;

public record GetPollByIdQuery(int Id) : ICachedQuery<Result<PollResponse>>
{
    public string CacheKey => CacheKeys.PollById(Id);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    public TimeSpan? LocalCacheExpiration => TimeSpan.FromMinutes(2);
}