using SurveyBasket.Application.Features.Results.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.Application.Features.Results.Queries.GetVotesPerDay;

public class GetVotesPerDayQueryHandler(
    IResultRepository resultRepository,
    IPollRepository pollRepository)
    : IRequestHandler<GetVotesPerDayQuery, Result<IEnumerable<VotesPerDayResponseDto>>>
{
    public async Task<Result<IEnumerable<VotesPerDayResponseDto>>> Handle(
        GetVotesPerDayQuery request,
        CancellationToken cancellationToken)
    {
        var pollExists = await pollRepository.ExistsAsync(request.PollId, cancellationToken);
        if (!pollExists)
            return Result.Failure<IEnumerable<VotesPerDayResponseDto>>(PollErrors.NotFound());

        var votes = await resultRepository.GetVotesByPollIdAsync(request.PollId, cancellationToken);

        var votesPerDay = votes
            .GroupBy(v => DateOnly.FromDateTime(v.SubmittedOn))
            .Select(g => new VotesPerDayResponseDto(g.Key, g.Count()))
            .ToList();

        return Result.Success<IEnumerable<VotesPerDayResponseDto>>(votesPerDay);
    }
}