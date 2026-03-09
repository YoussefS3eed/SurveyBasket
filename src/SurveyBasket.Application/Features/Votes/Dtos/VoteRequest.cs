namespace SurveyBasket.Application.Features.Votes.Dtos;

public record VoteRequest(
    IEnumerable<VoteAnswerRequest> Answers
);