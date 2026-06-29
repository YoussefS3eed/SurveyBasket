namespace SurveyBasket.Application.Features.Votes.Dtos;

public record VoteRequestDto(
    IEnumerable<VoteAnswerRequestDto> Answers
);