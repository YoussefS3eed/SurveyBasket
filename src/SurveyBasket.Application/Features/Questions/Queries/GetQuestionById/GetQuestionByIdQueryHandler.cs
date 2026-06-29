using SurveyBasket.Application.Features.Questions.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Questions.Queries.GetQuestionById;

internal sealed class GetQuestionByIdQueryHandler(IQuestionRepository questionRepository)
    : IRequestHandler<GetQuestionByIdQuery, Result<QuestionResponseDto>>
{
    public async Task<Result<QuestionResponseDto>> Handle(GetQuestionByIdQuery request, CancellationToken cancellationToken)
    {
        var question = await questionRepository.GetByIdAsync(request.PollId, request.Id, true, cancellationToken);

        if (question is null)
            return Result.Failure<QuestionResponseDto>(QuestionErrors.NotFound());

        return Result.Success(question.Adapt<QuestionResponseDto>());
    }
}