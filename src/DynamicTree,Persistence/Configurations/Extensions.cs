using DynamicTree.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicTree.Persistence.Configurations;

internal static class Extensions
{
    internal static void HasIdentityKey<T, T1>(this EntityTypeBuilder<T> builder, string keyType)
        where T : class, IIdentityEntity<T1>
        where T1: struct
    {
        builder.HasKey(k => k.Id);

        builder.Property(p => p.Id)
            .HasColumnName("Id")
            .HasColumnType(keyType)
            .IsRequired()
            .ValueGeneratedOnAdd();
    }

    internal static void HasAuditableColumns<T>(this EntityTypeBuilder<T> builder)
        where T : class, IAuditableEntity
    {
        builder.HasLastModifiedColumn();
    }

    internal static void HasLastModifiedColumn<T>(this EntityTypeBuilder<T> builder)
        where T : class, ICreatedAtEntity
    {
        builder.Property(p => p.CreatedAt)
            .HasColumnName("CreatedAt")
            .HasColumnType("datetime")
            .IsRequired();
    }
}