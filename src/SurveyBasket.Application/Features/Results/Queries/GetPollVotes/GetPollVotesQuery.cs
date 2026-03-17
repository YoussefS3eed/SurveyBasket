using SurveyBasket.Application.Common.Caching;
using SurveyBasket.Application.Common.Constants;
using SurveyBasket.Application.Features.Results.Dtos;

namespace SurveyBasket.Application.Features.Results.Queries.GetPollVotes;

public record GetPollVotesQuery(int PollId)
    : ICachedQuery<Result<PollVotesResponse>>
{
    public string CacheKey => CacheKeys.PollById(PollId);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(30);
    public TimeSpan? LocalCacheExpiration => TimeSpan.FromMinutes(5);
}