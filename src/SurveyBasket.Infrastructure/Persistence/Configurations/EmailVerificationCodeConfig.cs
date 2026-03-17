using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Infrastructure.Persistence.Configurations;

public class EmailVerificationCodeConfig : IEntityTypeConfiguration<EmailVerificationCode>
{
    public void Configure(EntityTypeBuilder<EmailVerificationCode> builder)
    {
        builder.ToTable("EmailVerificationCodes");

        builder.Property(evc => evc.Code).HasMaxLength(10).IsRequired();
        builder.Property(evc => evc.NewEmail).HasMaxLength(256).IsRequired();
        builder.Property(evc => evc.ExpiresAt).IsRequired();
        builder.Property(evc => evc.IsUsed).HasDefaultValue(false);
        builder.Property(evc => evc.CreatedAt).IsRequired();

        builder.HasIndex(evc => evc.UserId);
        builder.HasIndex(evc => evc.Code);

        builder.HasOne(evc => evc.User)
            .WithMany()
            .HasForeignKey(evc => evc.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
