using SurveyBasket.Application.Common.Caching;
using SurveyBasket.Application.Common.Constants;
using SurveyBasket.Application.Features.Results.Dtos;

namespace SurveyBasket.Application.Features.Results.Queries.GetVotesPerQuestion;

public record GetVotesPerQuestionQuery(int PollId) : ICachedQuery<Result<IEnumerable<VotesPerQuestionResponse>>>
{
    public string CacheKey => CacheKeys.VotesPerQuestion(PollId);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(20);
    public TimeSpan? LocalCacheExpiration => TimeSpan.FromMinutes(5);
}