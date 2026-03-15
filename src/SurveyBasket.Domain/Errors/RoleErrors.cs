namespace SurveyBasket.Domain.Errors;

public static class RoleErrors
{
    public static Error RoleNotFound(string id) =>
        Error.NotFound("Role.RoleNotFound", $"Role with id '{id}' was not found");

    public static Error DuplicateRole(string name) =>
        Error.Conflict("Role.DuplicateRole", $"Another role with the name '{name}' already exists");

    public static Error InvalidPermissions =>
        Error.Validation("Role.InvalidPermissions", "One or more permissions are not valid");

    public static Error CannotDeleteDefaultRole =>
        Error.Conflict("Role.CannotDeleteDefaultRole", "Cannot delete a default role");
}
