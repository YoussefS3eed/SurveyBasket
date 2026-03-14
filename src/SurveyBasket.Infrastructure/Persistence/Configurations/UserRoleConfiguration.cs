using Microsoft.AspNetCore.Identity;
using SurveyBasket.Application.Common.Contracts;

namespace SurveyBasket.Infrastructure.Persistence.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    //public const string AdminUserId = "6dc6528a-b280-4770-9eae-82671ee81ef7";

    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        builder.HasData(new IdentityUserRole<string>
        {
            UserId = DefaultUsers.AdminId,
            RoleId = DefaultRoles.AdminRoleId
        });
    }
}
