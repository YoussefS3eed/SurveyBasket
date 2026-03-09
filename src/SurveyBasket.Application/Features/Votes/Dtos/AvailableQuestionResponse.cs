using SurveyBasket.Application.Features.Answers.Dtos;

namespace SurveyBasket.Application.Features.Votes.Dtos;

public record AvailableQuestionResponse(
    int Id,
    string Content,
    IEnumerable<AnswerResponse> Answers
);