using SurveyBasket.Application.Abstractions.Caching;

namespace SurveyBasket.Application.Polls.Commands.CreatePoll;

public record CreatePollCommand(
    string Title,
    string Summary,
    bool IsPublished,
    DateOnly StartsAt,
    DateOnly EndsAt
) : IInvalidateCacheCommand<Result<PollDto>>
{
    public IEnumerable<string> CacheKeys => [ConstCacheKeys.AllPolls];
}