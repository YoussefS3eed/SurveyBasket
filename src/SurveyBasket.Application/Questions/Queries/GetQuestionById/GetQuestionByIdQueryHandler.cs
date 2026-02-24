using SurveyBasket.Application.Questions.Dtos;
using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Application.Questions.Queries.GetQuestionById;

internal sealed class GetQuestionByIdQueryHandler(IQuestionRepository questionRepository)
    : IRequestHandler<GetQuestionByIdQuery, Result<QuestionResponse>>
{
    public async Task<Result<QuestionResponse>> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var question = await questionRepository.GetByIdAsync(request.PollId, request.Id, true, cancellationToken);

        if (question is null)
            return Result.Failure<QuestionResponse>(QuestionErrors.QuestionNotFound);

        return Result.Success(question.Adapt<QuestionResponse>());
    }
}