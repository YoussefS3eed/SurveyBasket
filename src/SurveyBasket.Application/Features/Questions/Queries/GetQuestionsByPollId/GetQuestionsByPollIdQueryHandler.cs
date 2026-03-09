using SurveyBasket.Application.Features.Questions.Dtos;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Questions.Queries.GetQuestionsByPollId;

internal sealed class GetQuestionsByPollIdQueryHandler(IQuestionRepository questionRepository, IPollRepository pollRepository)
    : IRequestHandler<GetQuestionsByPollIdQuery, Result<IEnumerable<QuestionResponse>>>
{
    public async Task<Result<IEnumerable<QuestionResponse>>> Handle(GetQuestionsByPollIdQuery request, CancellationToken cancellationToken)
    {
        var pollExists = await pollRepository.ExistsAsync(request.PollId, cancellationToken);

        if (!pollExists)
            return Result.Failure<IEnumerable<QuestionResponse>>(PollErrors.NotFound());

        var questions = await questionRepository.GetByPollIdAsync(request.PollId, cancellationToken);

        return Result.Success(questions.Adapt<IEnumerable<QuestionResponse>>());
    }
}