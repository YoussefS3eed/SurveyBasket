using SurveyBasket.Application.Common.Models;
using SurveyBasket.Application.Features.Questions.Dtos;
using SurveyBasket.Domain.Interfaces.Repositories;
using System.Linq.Dynamic.Core;

namespace SurveyBasket.Application.Features.Questions.Queries.GetQuestionsByPollId;

internal sealed class GetQuestionsByPollIdQueryHandler(IQuestionRepository questionRepository, IPollRepository pollRepository)
    : IRequestHandler<GetQuestionsByPollIdQuery, Result<PaginatedList<QuestionResponseDto>>>
{
    public async Task<Result<PaginatedList<QuestionResponseDto>>> Handle(GetQuestionsByPollIdQuery request, CancellationToken cancellationToken)
    {
        var pollExists = await pollRepository.ExistsAsync(request.PollId, cancellationToken);

        if (!pollExists)
            return PollErrors.NotFound();

        var query = await questionRepository.GetByPollIdAsync(request.PollId, cancellationToken);

        if (!string.IsNullOrEmpty(request.Filters.SearchValue))
        {
            query = query.Where(x => x.Content.Contains(request.Filters.SearchValue));
        }

        if (!string.IsNullOrEmpty(request.Filters.SortColumn))
        {
            query = query.OrderBy($"{request.Filters.SortColumn} {request.Filters.SortDirection}");
        }

        var projectedQuery = query.ProjectToType<QuestionResponseDto>();
        var questions = await PaginatedList<QuestionResponseDto>.CreateAsync(projectedQuery, request.Filters.PageNumber, request.Filters.PageSize, cancellationToken);

        return Result.Success(questions);
    }
}