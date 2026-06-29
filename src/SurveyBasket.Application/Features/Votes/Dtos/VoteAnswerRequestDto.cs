namespace SurveyBasket.Application.Features.Votes.Dtos;

public record VoteAnswerRequestDto(
    int QuestionId,
    int AnswerId
);