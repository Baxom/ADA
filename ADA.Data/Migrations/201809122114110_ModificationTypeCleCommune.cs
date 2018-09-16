namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificationTypeCleCommune : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Pretre", "CommuneId", "dbo.Commune");
            DropIndex("dbo.Pretre", new[] { "CommuneId" });
            RenameColumn(table: "dbo.Pretre", name: "CommuneId", newName: "CommuneCodeInsee");
            DropPrimaryKey("dbo.Commune");
            AlterColumn("dbo.Commune", "CodeInsee", c => c.String(nullable: false, maxLength: 6));
            AlterColumn("dbo.Pretre", "CommuneCodeInsee", c => c.String(maxLength: 6));
            AddPrimaryKey("dbo.Commune", "CodeInsee");
            CreateIndex("dbo.Pretre", "CommuneCodeInsee");
            AddForeignKey("dbo.Pretre", "CommuneCodeInsee", "dbo.Commune", "CodeInsee");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pretre", "CommuneCodeInsee", "dbo.Commune");
            DropIndex("dbo.Pretre", new[] { "CommuneCodeInsee" });
            DropPrimaryKey("dbo.Commune");
            AlterColumn("dbo.Pretre", "CommuneCodeInsee", c => c.Int());
            AlterColumn("dbo.Commune", "CodeInsee", c => c.String());
            AddPrimaryKey("dbo.Commune", "Id");
            RenameColumn(table: "dbo.Pretre", name: "CommuneCodeInsee", newName: "CommuneId");
            CreateIndex("dbo.Pretre", "CommuneId");
            AddForeignKey("dbo.Pretre", "CommuneId", "dbo.Commune", "Id");
        }
    }
}
