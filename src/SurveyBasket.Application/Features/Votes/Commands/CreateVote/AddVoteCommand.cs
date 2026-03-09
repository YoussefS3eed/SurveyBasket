using SurveyBasket.Application.Features.Votes.Dtos;
using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Votes.Commands.CreateVote;

public record AddVoteCommand(
    int PollId,
    IEnumerable<VoteAnswerRequest> Answers
) : IRequest<Result>;