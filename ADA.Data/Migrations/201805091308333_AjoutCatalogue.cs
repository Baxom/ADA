namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutCatalogue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Catalogue",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Cote = c.String(),
                        Titre = c.String(),
                        SousSerieId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Index", t => t.SousSerieId)
                .Index(t => t.SousSerieId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Catalogue", "SousSerieId", "dbo.Index");
            DropIndex("dbo.Catalogue", new[] { "SousSerieId" });
            DropTable("dbo.Catalogue");
        }
    }
}
