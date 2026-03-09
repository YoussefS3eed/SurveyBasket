using SurveyBasket.Application.Common.Caching;
using SurveyBasket.Application.Common.Constants;
using SurveyBasket.Application.Features.Questions.Dtos;
using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Questions.Queries.GetQuestionsByPollId;

public record GetQuestionsByPollIdQuery(int PollId) : ICachedQuery<Result<IEnumerable<QuestionResponse>>>
{
    public string CacheKey => CacheKeys.QuestionsByPollId(PollId);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(15);
    public TimeSpan? LocalCacheExpiration => TimeSpan.FromMinutes(3);
}