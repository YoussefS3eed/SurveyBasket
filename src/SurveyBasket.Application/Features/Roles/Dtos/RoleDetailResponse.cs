namespace SurveyBasket.Application.Features.Roles.Dtos;

public record RoleDetailResponse(
    string Id,
    string Name,
    bool IsDeleted,
    IEnumerable<string> Permissions
);
