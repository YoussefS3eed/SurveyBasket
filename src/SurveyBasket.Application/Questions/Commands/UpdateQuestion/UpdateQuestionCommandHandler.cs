using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Application.Questions.Commands.UpdateQuestion;

internal class UpdateQuestionCommandHandler(IQuestionRepository questionRepository, IPollRepository pollRepository)
    : IRequestHandler<UpdateQuestionCommand, Result>
{
    public async Task<Result> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var existPoll = await pollRepository.ExistsAsync(request.PollId, cancellationToken);

        if (!existPoll)
            return Result.Failure(PollErrors.PollNotFound);

        var question = await questionRepository.GetByIdAsync(request.PollId, request.Id, true, cancellationToken);

        if (question is null)
            return Result.Failure(QuestionErrors.QuestionNotFound);

        var questionIsExists = await questionRepository.ExistsByContentExceptIdAsync(request.PollId, request.Content, request.Id, cancellationToken);

        if (questionIsExists)
            return Result.Failure(QuestionErrors.DuplicatedQuestionContent);

        question.Content = request.Content;

        // Merge answers
        var currentAnswers = question.Answers.Select(a => a.Content).ToList();
        var newAnswers = request.Answers.Except(currentAnswers).ToList();
        foreach (var answer in newAnswers)
            question.Answers.Add(new Answer { Content = answer });

        foreach (var answer in question.Answers)
            answer.IsActive = request.Answers.Contains(answer.Content);

        await questionRepository.UpdateAsync(question, cancellationToken);
        return Result.Success();
    }
}