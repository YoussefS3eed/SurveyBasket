namespace SurveyBasket.Application.Features.Roles.Dtos;

public record RoleDetailResponseDto(
    string Id,
    string Name,
    bool IsDeleted,
    IEnumerable<string> Permissions
);
