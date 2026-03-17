using SurveyBasket.Application.Common.Caching;

namespace SurveyBasket.Application.Features.Polls.Commands.UpdatePoll;

public record UpdatePollCommand(
    int Id,
    string Title,
    string Summary,
    bool IsPublished,
    DateOnly StartsAt,
    DateOnly EndsAt
) : ICacheInvalidationCommand<Result> // 
{
    public IEnumerable<string> CacheKeys =>
    [
        Common.Constants.CacheKeys.AllPolls,
        Common.Constants.CacheKeys.CurrentPolls,
        Common.Constants.CacheKeys.PollById(Id)
    ];
}