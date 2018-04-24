namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreationBase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bibliotheque",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Auteur = c.String(),
                        Titre = c.String(),
                        Annee = c.String(),
                        Aspect = c.String(),
                        Epi = c.String(),
                        Zone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Lieu",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Nom = c.String(),
                        TypeLieuId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TypeLieu", t => t.TypeLieuId)
                .Index(t => t.TypeLieuId);
            
            CreateTable(
                "dbo.TypeLieu",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        TypeRecherche = c.Int(nullable: false),
                        Nom = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pretre",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Nom = c.String(),
                        Prenom = c.String(),
                        Chanoine = c.String(),
                        AnneeNaissance = c.Int(),
                        AnneeDeces = c.Int(),
                        Decede = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PretreDocument",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Nom = c.String(),
                        PretreId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pretre", t => t.PretreId)
                .Index(t => t.PretreId);
            
            CreateTable(
                "dbo.PretreFonctionLieu",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AnneeDebut = c.Int(),
                        AnneeFin = c.Int(),
                        FonctionId = c.Int(),
                        LieuId = c.Int(),
                        PretreId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fonction", t => t.FonctionId)
                .ForeignKey("dbo.Lieu", t => t.LieuId)
                .ForeignKey("dbo.Pretre", t => t.PretreId)
                .Index(t => t.FonctionId)
                .Index(t => t.LieuId)
                .Index(t => t.PretreId);
            
            CreateTable(
                "dbo.Fonction",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Nom = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PretrePhoto",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        NomFichier = c.String(),
                        PretreId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Pretre", t => t.PretreId)
                .Index(t => t.PretreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PretrePhoto", "PretreId", "dbo.Pretre");
            DropForeignKey("dbo.PretreFonctionLieu", "PretreId", "dbo.Pretre");
            DropForeignKey("dbo.PretreFonctionLieu", "LieuId", "dbo.Lieu");
            DropForeignKey("dbo.PretreFonctionLieu", "FonctionId", "dbo.Fonction");
            DropForeignKey("dbo.PretreDocument", "PretreId", "dbo.Pretre");
            DropForeignKey("dbo.Lieu", "TypeLieuId", "dbo.TypeLieu");
            DropIndex("dbo.PretrePhoto", new[] { "PretreId" });
            DropIndex("dbo.PretreFonctionLieu", new[] { "PretreId" });
            DropIndex("dbo.PretreFonctionLieu", new[] { "LieuId" });
            DropIndex("dbo.PretreFonctionLieu", new[] { "FonctionId" });
            DropIndex("dbo.PretreDocument", new[] { "PretreId" });
            DropIndex("dbo.Lieu", new[] { "TypeLieuId" });
            DropTable("dbo.PretrePhoto");
            DropTable("dbo.Fonction");
            DropTable("dbo.PretreFonctionLieu");
            DropTable("dbo.PretreDocument");
            DropTable("dbo.Pretre");
            DropTable("dbo.TypeLieu");
            DropTable("dbo.Lieu");
            DropTable("dbo.Bibliotheque");
        }
    }
}
