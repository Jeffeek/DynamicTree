using FluentMigrator;
using FluentMigrator.SqlServer;

namespace DynamicTree.Persistence.Migrations.Migrations._2023._04._24;

[Migration(2023_04_24_1, "Create TreeNode Table")]
public class CreateTreeNodeTable : Migration
{
    public override void Up()
    {
        Create.Table("TreeNode")
            .WithColumn("Id").AsInt64().NotNullable().PrimaryKey("PK_CreateTreeNodeTable_Id").Identity(1, 1)
            .WithColumn("Name").AsString(512).NotNullable()
            .WithColumn("ParentNodeId").AsInt64().Nullable().ForeignKey("FK_TreeNode_TreeNode_ParentNodeId_Id", "TreeNode", "Id");
    }

    public override void Down()
    {
        Delete.ForeignKey("FK_TreeNode_TreeNode_ParentNodeId_Id").OnTable("TreeNode");
        Delete.PrimaryKey("PK_CreateTreeNodeTable_Id").FromTable("TreeNode").Column("Id");
        Delete.Table("TreeNode");
    }
}