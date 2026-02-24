namespace SurveyBasket.Infrastructure.Configurations;

public class ApplicayionUserConfig : IEntityTypeConfiguration<ApplicationUser>
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
    }
}
