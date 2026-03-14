using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Entities;
using SurveyBasket.Domain.Interfaces;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Questions.Commands.UpdateQuestion;

internal sealed class UpdateQuestionCommandHandler(
    IQuestionRepository questionRepository,
    IPollRepository pollRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateQuestionCommand, Result>
{
    public async Task<Result> Handle(
        UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var pollExists = await pollRepository.ExistsAsync(request.PollId, cancellationToken);

        if (!pollExists)
            return Result.Failure(PollErrors.NotFound(request.PollId));

        var question = await questionRepository.GetByIdAsync(
            request.PollId, request.Id, true, cancellationToken);

        if (question is null)
            return Result.Failure(QuestionErrors.NotFound());

        var isDuplicate = await questionRepository.ExistsByContentExceptIdAsync(
            request.PollId, request.Content, request.Id, cancellationToken);

        if (isDuplicate)
            return Result.Failure(QuestionErrors.DuplicatedQuestionContent);

        question.Content = request.Content;

        // Add newly submitted answers that don't already exist
        var currentAnswerContents = question.Answers.Select(a => a.Content).ToList();
        var newAnswers = request.Answers.Except(currentAnswerContents).ToList();

        foreach (var answer in newAnswers)
            question.Answers.Add(new Answer { Content = answer });

        // Toggle IsActive based on whether the answer is in the new list
        foreach (var answer in question.Answers)
            answer.IsActive = request.Answers.Contains(answer.Content);

        questionRepository.Update(question);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}