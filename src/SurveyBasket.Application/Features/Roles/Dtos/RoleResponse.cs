namespace SurveyBasket.Application.Features.Roles.Dtos;

public record RoleResponse(
    string Id,
    string Name,
    bool IsDeleted
);
