using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Infrastructure.Configurations;

public class ApplicayionUserConfig : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(x => x.FirstName).HasMaxLength(100);
        builder.Property(x => x.LastName).HasMaxLength(100);
    }
}
