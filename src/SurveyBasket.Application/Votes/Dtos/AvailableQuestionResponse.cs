using SurveyBasket.Application.Answers.Dtos;

namespace SurveyBasket.Application.Votes.Dtos;

public record AvailableQuestionResponse(
    int Id,
    string Content,
    IEnumerable<AnswerResponse> Answers
);