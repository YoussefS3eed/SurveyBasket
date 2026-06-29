namespace SurveyBasket.Application.Features.Results.Dtos;

public record PollVotesResponseDto(
    string Title,
    IEnumerable<VoteResponseDto> Votes
);