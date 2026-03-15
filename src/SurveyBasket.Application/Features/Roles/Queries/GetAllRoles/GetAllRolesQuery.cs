using SurveyBasket.Application.Features.Roles.Dtos;
using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Roles.Queries.GetAllRoles;

public record GetAllRolesQuery(bool? IncludeDisabled = false) 
    : IRequest<Result<IEnumerable<RoleResponse>>>;
