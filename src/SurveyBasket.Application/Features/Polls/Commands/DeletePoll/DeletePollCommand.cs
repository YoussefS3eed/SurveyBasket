using SurveyBasket.Application.Common.Caching;

namespace SurveyBasket.Application.Features.Polls.Commands.DeletePoll;

public record DeletePollCommand(int Id) : ICacheInvalidationCommand<Result>
{
    public IEnumerable<string> CacheKeys =>
    [
        Common.Constants.CacheKeys.AllPolls,
        Common.Constants.CacheKeys.CurrentPolls,
        Common.Constants.CacheKeys.PollById(Id)
    ];
}