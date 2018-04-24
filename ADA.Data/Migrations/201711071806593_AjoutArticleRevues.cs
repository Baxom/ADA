namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutArticleRevues : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleRevue",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Titre = c.String(),
                        Pages = c.String(),
                        Annee = c.Int(nullable: false),
                        Auteur = c.String(),
                        RevueId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Revue", t => t.RevueId)
                .Index(t => t.RevueId);
            
            CreateTable(
                "dbo.Revue",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Code = c.String(),
                        Nom = c.String(),
                        NomDossierModele = c.String(),
                        NomFichierModele = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArticleRevue", "RevueId", "dbo.Revue");
            DropIndex("dbo.ArticleRevue", new[] { "RevueId" });
            DropTable("dbo.Revue");
            DropTable("dbo.ArticleRevue");
        }
    }
}
