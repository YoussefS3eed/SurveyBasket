namespace SurveyBasket.Domain.Common.BaseEntities;

/// <summary>
/// Navigation properties to ApplicationUser are intentional.
/// Both AuditableEntity and ApplicationUser live in Domain,
/// so this does not create a cross-layer dependency.
/// EF Core resolves the FK via ApplicationUserConfig.
/// </summary>
public abstract class AuditableEntity
{
    public string CreatedById { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public string? UpdatedById { get; set; }
    public DateTime? UpdatedOn { get; set; }

    // Navigation properties — fine here, both types are Domain entities
    public ApplicationUser CreatedBy { get; set; } = default!;
    public ApplicationUser? UpdatedBy { get; set; }
}