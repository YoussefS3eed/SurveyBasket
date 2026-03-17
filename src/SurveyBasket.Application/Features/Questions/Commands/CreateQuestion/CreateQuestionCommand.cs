using SurveyBasket.Application.Common.Caching;
using SurveyBasket.Application.Features.Questions.Dtos;

namespace SurveyBasket.Application.Features.Questions.Commands.CreateQuestion;

public record CreateQuestionCommand(int PollId, string Content, IEnumerable<string> Answers) : ICacheInvalidationCommand<Result<QuestionResponse>>
{
    public IEnumerable<string> CacheKeys => [Common.Constants.CacheKeys.QuestionsByPollIdPrefix(PollId)];
}


