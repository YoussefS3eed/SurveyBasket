using SurveyBasket.Application.Common.Caching;
using SurveyBasket.Application.Common.Constants;
using SurveyBasket.Application.Features.Polls.Dtos;
using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Polls.Queries.GetPollById;

public record GetPollByIdQuery(int Id) : ICachedQuery<Result<PollDto>>
{
    public string CacheKey => CacheKeys.PollById(Id);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
    public TimeSpan? LocalCacheExpiration => TimeSpan.FromMinutes(2);
}