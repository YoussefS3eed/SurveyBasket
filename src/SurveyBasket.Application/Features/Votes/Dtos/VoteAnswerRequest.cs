namespace SurveyBasket.Application.Features.Votes.Dtos;

public record VoteAnswerRequest(
    int QuestionId,
    int AnswerId
);