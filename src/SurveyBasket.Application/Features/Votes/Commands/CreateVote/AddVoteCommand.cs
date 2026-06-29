using SurveyBasket.Application.Features.Votes.Dtos;

namespace SurveyBasket.Application.Features.Votes.Commands.CreateVote;

public record AddVoteCommand(
    int PollId,
    IEnumerable<VoteAnswerRequestDto> Answers
) : IRequest<Result>;