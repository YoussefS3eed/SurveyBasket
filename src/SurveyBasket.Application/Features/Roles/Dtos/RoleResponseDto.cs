namespace SurveyBasket.Application.Features.Roles.Dtos;

public record RoleResponseDto(
    string Id,
    string Name,
    bool IsDeleted
);
