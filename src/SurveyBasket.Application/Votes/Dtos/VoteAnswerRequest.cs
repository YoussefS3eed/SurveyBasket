namespace SurveyBasket.Application.Votes.Dtos;

public record VoteAnswerRequest(
    int QuestionId,
    int AnswerId
);