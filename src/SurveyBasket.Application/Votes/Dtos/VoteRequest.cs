namespace SurveyBasket.Application.Votes.Dtos;

public record VoteRequest(
    IEnumerable<VoteAnswerRequest> Answers
);