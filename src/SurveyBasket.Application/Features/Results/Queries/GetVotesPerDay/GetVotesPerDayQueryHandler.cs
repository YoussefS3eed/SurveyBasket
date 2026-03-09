using SurveyBasket.Application.Features.Results.Dtos;
using SurveyBasket.Domain.Common.Models;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Results.Queries.GetVotesPerDay;

public class GetVotesPerDayQueryHandler(
    IResultRepository resultRepository,
    IPollRepository pollRepository)
    : IRequestHandler<GetVotesPerDayQuery, Result<IEnumerable<VotesPerDayResponse>>>
{
    public async Task<Result<IEnumerable<VotesPerDayResponse>>> Handle(
        GetVotesPerDayQuery request,
        CancellationToken cancellationToken)
    {
        var pollExists = await pollRepository.ExistsAsync(request.PollId, cancellationToken);
        if (!pollExists)
            return Result.Failure<IEnumerable<VotesPerDayResponse>>(PollErrors.NotFound());

        var votes = await resultRepository.GetVotesByPollIdAsync(request.PollId, cancellationToken);

        var votesPerDay = votes
            .GroupBy(v => DateOnly.FromDateTime(v.SubmittedOn))
            .Select(g => new VotesPerDayResponse(g.Key, g.Count()))
            .ToList();

        return Result.Success<IEnumerable<VotesPerDayResponse>>(votesPerDay);
    }
}