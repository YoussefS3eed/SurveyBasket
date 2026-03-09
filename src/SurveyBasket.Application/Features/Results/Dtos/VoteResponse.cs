namespace SurveyBasket.Application.Features.Results.Dtos;

public record VoteResponse(
    string VoterName,
    DateTime VoteDate,
    IEnumerable<QuestionAnswerResponse> SelectedAnswers
);