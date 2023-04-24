using FluentMigrator.Builders.Create.Table;

namespace DynamicTree.Persistence.Migrations.Migrations;

internal static class Extensions
{
    internal static ICreateTableWithColumnSyntax WithCreatedAtColumn(this ICreateTableWithColumnSyntax syntax)
        => syntax.WithColumn("CreatedAt").AsDateTime().NotNullable();
}