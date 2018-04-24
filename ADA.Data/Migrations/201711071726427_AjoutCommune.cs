namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutCommune : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Commune",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        CodeInsee = c.Int(nullable: false),
                        Nom = c.String(),
                        CodePostal = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Pretre", "CommuneId", c => c.Int());
            CreateIndex("dbo.Pretre", "CommuneId");
            AddForeignKey("dbo.Pretre", "CommuneId", "dbo.Commune", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Pretre", "CommuneId", "dbo.Commune");
            DropIndex("dbo.Pretre", new[] { "CommuneId" });
            DropColumn("dbo.Pretre", "CommuneId");
            DropTable("dbo.Commune");
        }
    }
}
