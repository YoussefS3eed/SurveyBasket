namespace SurveyBasket.Application.Features.Results.Dtos;

public record VotesPerQuestionResponse(
    string Question,
    IEnumerable<VotesPerAnswerResponse> SelectedAnswers
);