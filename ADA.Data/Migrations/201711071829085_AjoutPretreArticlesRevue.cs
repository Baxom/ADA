namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutPretreArticlesRevue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PretreArticleRevue",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Annee = c.String(),
                        Pages = c.String(),
                        RevueId = c.Int(),
                        PretreId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Revue", t => t.RevueId)
                .ForeignKey("dbo.Pretre", t => t.PretreId)
                .Index(t => t.RevueId)
                .Index(t => t.PretreId);
            
            AlterColumn("dbo.ArticleRevue", "Annee", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PretreArticleRevue", "PretreId", "dbo.Pretre");
            DropForeignKey("dbo.PretreArticleRevue", "RevueId", "dbo.Revue");
            DropIndex("dbo.PretreArticleRevue", new[] { "PretreId" });
            DropIndex("dbo.PretreArticleRevue", new[] { "RevueId" });
            AlterColumn("dbo.ArticleRevue", "Annee", c => c.Int(nullable: false));
            DropTable("dbo.PretreArticleRevue");
        }
    }
}
