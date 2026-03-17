using SurveyBasket.Application.Features.Questions.Dtos;
using SurveyBasket.Domain.Entities;
using SurveyBasket.Domain.Interfaces;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Questions.Commands.CreateQuestion;

internal sealed class CreateQuestionCommandHandler(
    IQuestionRepository questionRepository,
    IPollRepository pollRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateQuestionCommand, Result<QuestionResponse>>
{
    public async Task<Result<QuestionResponse>> Handle(
        CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var pollExists = await pollRepository.ExistsAsync(request.PollId, cancellationToken);

        if (!pollExists)
            return Result.Failure<QuestionResponse>(PollErrors.NotFound(request.PollId));

        var isDuplicate = await questionRepository.ExistsByContentExceptIdAsync(
            request.PollId, request.Content, null, cancellationToken);

        if (isDuplicate)
            return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);

        var question = new Question
        {
            PollId = request.PollId,
            Content = request.Content,
            Answers = request.Answers.Select(a => new Answer { Content = a }).ToList()
        };

        await questionRepository.AddAsync(question, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(question.Adapt<QuestionResponse>());
    }
}
