using SurveyBasket.Application.Common.Caching;
using SurveyBasket.Application.Common.Constants;
using SurveyBasket.Application.Common.Models;
using SurveyBasket.Application.Features.Questions.Dtos;

namespace SurveyBasket.Application.Features.Questions.Queries.GetQuestionsByPollId;

public record GetQuestionsByPollIdQuery(int PollId, RequestFilters Filters) : ICachedQuery<Result<PaginatedList<QuestionResponse>>>
{
    public string CacheKey => CacheKeys.QuestionsByPollId(PollId, Filters.PageNumber, Filters.PageSize, Filters.SearchValue, Filters.SortColumn, Filters.SortDirection);
    public TimeSpan? Expiration => TimeSpan.FromMinutes(15);
    public TimeSpan? LocalCacheExpiration => TimeSpan.FromMinutes(3);
}