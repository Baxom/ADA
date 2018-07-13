namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjourSousSerieCatalogue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SousSerie",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Titre = c.String(),
                        SerieId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Serie", t => t.SerieId, cascadeDelete: true)
                .Index(t => t.SerieId);
            
            CreateTable(
                "dbo.Serie",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Titre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Catalogue", "Date", c => c.String());
            AddColumn("dbo.Catalogue", "SousSerieId", c => c.Int());
            CreateIndex("dbo.Catalogue", "SousSerieId");
            AddForeignKey("dbo.Catalogue", "SousSerieId", "dbo.SousSerie", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Catalogue", "SousSerieId", "dbo.SousSerie");
            DropForeignKey("dbo.SousSerie", "SerieId", "dbo.Serie");
            DropIndex("dbo.SousSerie", new[] { "SerieId" });
            DropIndex("dbo.Catalogue", new[] { "SousSerieId" });
            DropColumn("dbo.Catalogue", "SousSerieId");
            DropColumn("dbo.Catalogue", "Date");
            DropTable("dbo.Serie");
            DropTable("dbo.SousSerie");
        }
    }
}
