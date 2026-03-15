using SurveyBasket.Application.Features.Roles.Dtos;

namespace SurveyBasket.Application.Features.Roles.Queries.GetRoleById;

public record GetRoleByIdQuery(string Id) 
    : IRequest<Result<RoleDetailResponse>>;
