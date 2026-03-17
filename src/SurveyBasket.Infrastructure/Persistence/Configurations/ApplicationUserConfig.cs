using SurveyBasket.Application.Common.Contracts;

namespace SurveyBasket.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);

        // Configure RefreshTokens as owned entity
        builder.OwnsMany(x => x.RefreshTokens, refreshToken =>
        {
            refreshToken.ToTable("RefreshTokens");
            refreshToken.Property(rt => rt.Token).HasMaxLength(500);
            refreshToken.Property(rt => rt.ExpiresOn).IsRequired();
            refreshToken.Property(rt => rt.CreatedOn).IsRequired();
            refreshToken.Property(rt => rt.RevokedOn);
            refreshToken.WithOwner().HasForeignKey("UserId");
        });

        // Pre-hashed password for "P@ssword123" - static value to avoid model changes
        const string adminPasswordHash = "AQAAAAIAAYagAAAAEAR8j58/FfDjwiMZTAWfASuvRCmxY03FD9bMPfSdbfeVabrt+o5Z4GV3ij+M3Ncc2g==";

        builder.HasData(new ApplicationUser
        {
            Id = DefaultUsers.AdminId,
            FirstName = "Survey Basket",
            LastName = "Admin",
            UserName = DefaultUsers.AdminUserName,
            NormalizedUserName = DefaultUsers.AdminUserName.ToUpper(),
            Email = DefaultUsers.AdminEmail,
            NormalizedEmail = DefaultUsers.AdminEmail.ToUpper(),
            SecurityStamp = DefaultUsers.AdminSecurityStamp,
            ConcurrencyStamp = DefaultUsers.AdminConcurrencyStamp,
            EmailConfirmed = true,
            PasswordHash = adminPasswordHash
        });
    }
}
