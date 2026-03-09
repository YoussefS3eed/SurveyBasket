namespace SurveyBasket.Application.Features.Results.Dtos;

public record PollVotesResponse(
    string Title,
    IEnumerable<VoteResponse> Votes
);