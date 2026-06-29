namespace SurveyBasket.Application.Features.Results.Dtos;

public record VoteResponseDto(
    string VoterName,
    DateTime VoteDate,
    IEnumerable<QuestionAnswerResponseDto> SelectedAnswers
);