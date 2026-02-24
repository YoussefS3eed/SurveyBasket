using SurveyBasket.Application.Questions.Dtos;
using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Application.Questions.Commands.CreateQuestion;

internal class CreateQuestionCommandHandler(IQuestionRepository questionRepository, IPollRepository pollRepository)
    : IRequestHandler<CreateQuestionCommand, Result<QuestionResponse>>
{
    public async Task<Result<QuestionResponse>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var existPoll = await pollRepository.ExistsAsync(request.PollId, cancellationToken);

        if (!existPoll)
            return Result.Failure<QuestionResponse>(PollErrors.PollNotFound);

        var questionIsExists = await questionRepository.ExistsByContentExceptIdAsync(request.PollId, request.Content, null, cancellationToken);

        if (questionIsExists)
            return Result.Failure<QuestionResponse>(QuestionErrors.DuplicatedQuestionContent);

        var question = new Question
        {
            PollId = request.PollId,
            Content = request.Content,
            Answers = request.Answers.Select(a => new Answer { Content = a }).ToList()
        };

        await questionRepository.CreateAsync(question, cancellationToken);
        return Result.Success(question.Adapt<QuestionResponse>());
    }
}
