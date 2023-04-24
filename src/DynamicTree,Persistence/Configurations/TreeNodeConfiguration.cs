using DynamicTree.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicTree.Persistence.Configurations;

public class TreeNodeConfiguration : IEntityTypeConfiguration<TreeNode>
{
    public void Configure(EntityTypeBuilder<TreeNode> builder)
    {
        builder.ToTable("TreeNode");
        builder.HasIdentityKey<TreeNode, long>("bigint");

        builder.Property(p => p.Name)
            .HasColumnName("Name")
            .HasColumnType("nvarchar")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(p => p.ParentNodeId)
            .HasColumnName("ParentNodeId")
            .HasColumnType("bigint")
            .IsRequired(false);

        builder.HasOne(p => p.ParentNode)
            .WithMany(p => p.Children)
            .HasForeignKey(k => k.ParentNodeId)
            .IsRequired(false);

        builder.HasMany(p => p.Children)
            .WithOne(p => p.ParentNode)
            .HasForeignKey(k => k.ParentNodeId)
            .IsRequired(false);
    }
}