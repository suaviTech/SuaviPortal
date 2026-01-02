using IzmPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IzmPortal.Infrastructure.Persistence.Configurations;

public class ApplicationShortcutConfiguration
    : IEntityTypeConfiguration<ApplicationShortcut>
{
    public void Configure(EntityTypeBuilder<ApplicationShortcut> builder)
    {
        builder.ToTable("ApplicationShortcuts");

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Icon)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Order)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);
    }
}
