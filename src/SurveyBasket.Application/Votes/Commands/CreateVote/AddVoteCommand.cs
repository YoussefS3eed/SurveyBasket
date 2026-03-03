using SurveyBasket.Application.Votes.Dtos;

namespace SurveyBasket.Application.Votes.Commands.CreateVote;

public record AddVoteCommand(int PollId, string UserId, VoteRequest Request) : IRequest<Result>;