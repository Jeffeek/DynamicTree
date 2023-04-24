using DynamicTree.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicTree.Persistence.Configurations;

public class JournalConfiguration : IEntityTypeConfiguration<Journal>
{
    public void Configure(EntityTypeBuilder<Journal> builder)
    {
        builder.ToTable("Journal");
        builder.HasIdentityKey<Journal, long>("bigint");
        builder.HasAuditableColumns();

        builder.Property(p => p.Text)
            .HasColumnName("Text")
            .HasColumnType("nvarchar")
            .HasMaxLength(int.MaxValue)
            .IsRequired();

        builder.Property(p => p.EventId)
            .HasColumnName("EventId")
            .HasColumnType("bigint")
            .IsRequired();
    }
}