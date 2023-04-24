using FluentMigrator;
using FluentMigrator.SqlServer;

namespace DynamicTree.Persistence.Migrations.Migrations._2023._04._24;

[Migration(2023_04_24_2, "Create Journal Table")]
public class CreateJournalTable : Migration
{
    public override void Up()
    {
        Create.Table("Journal")
            .WithColumn("Id").AsInt64().NotNullable().PrimaryKey("PK_Journal_Id").Identity(1, 1)
            .WithColumn("EventId").AsInt64().NotNullable()
            .WithColumn("Text").AsString(int.MaxValue).NotNullable()
            .WithCreatedAtColumn();
    }

    public override void Down()
    {
        Delete.PrimaryKey("PK_Journal_Id").FromTable("Journal").Column("Id");
        Delete.Table("Journal");
    }
}