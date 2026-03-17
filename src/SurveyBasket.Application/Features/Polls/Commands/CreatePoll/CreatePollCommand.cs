using SurveyBasket.Application.Common.Caching;
using SurveyBasket.Application.Features.Polls.Dtos;

namespace SurveyBasket.Application.Features.Polls.Commands.CreatePoll;

public record CreatePollCommand(
    string Title,
    string Summary,
    bool IsPublished,
    DateOnly StartsAt,
    DateOnly EndsAt
) : ICacheInvalidationCommand<Result<PollResponse>>
{
    public IEnumerable<string> CacheKeys =>
    [
        Common.Constants.CacheKeys.AllPolls,
        Common.Constants.CacheKeys.CurrentPolls
    ];
}