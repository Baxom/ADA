namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutActe : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Mariage",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AnneeRegistreParoissial = c.Int(nullable: false),
                        ParoisseRegistreId = c.Int(nullable: false),
                        Pages = c.String(),
                        DateMariageReligieux = c.DateTime(),
                        DateMariageCivil = c.DateTime(),
                        NomEpoux = c.String(),
                        PrenomEpoux = c.String(),
                        NomEpouse = c.String(),
                        PrenomEpouse = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lieu", t => t.ParoisseRegistreId, cascadeDelete: true)
                .Index(t => t.ParoisseRegistreId);
            
            CreateTable(
                "dbo.Bapteme",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AnneeRegistreParoissial = c.Int(nullable: false),
                        ParoisseRegistreId = c.Int(nullable: false),
                        Pages = c.String(),
                        Nom = c.String(nullable: false),
                        Prenom = c.String(nullable: false),
                        DateNaissance = c.DateTime(),
                        DateBapteme = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lieu", t => t.ParoisseRegistreId, cascadeDelete: true)
                .Index(t => t.ParoisseRegistreId);
            
            CreateTable(
                "dbo.Sepulture",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AnneeRegistreParoissial = c.Int(nullable: false),
                        ParoisseRegistreId = c.Int(nullable: false),
                        Pages = c.String(),
                        Nom = c.String(nullable: false),
                        Prenom = c.String(nullable: false),
                        DateDeces = c.DateTime(),
                        DateSepulture = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lieu", t => t.ParoisseRegistreId, cascadeDelete: true)
                .Index(t => t.ParoisseRegistreId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sepulture", "ParoisseRegistreId", "dbo.Lieu");
            DropForeignKey("dbo.Bapteme", "ParoisseRegistreId", "dbo.Lieu");
            DropForeignKey("dbo.Mariage", "ParoisseRegistreId", "dbo.Lieu");
            DropIndex("dbo.Sepulture", new[] { "ParoisseRegistreId" });
            DropIndex("dbo.Bapteme", new[] { "ParoisseRegistreId" });
            DropIndex("dbo.Mariage", new[] { "ParoisseRegistreId" });
            DropTable("dbo.Sepulture");
            DropTable("dbo.Bapteme");
            DropTable("dbo.Mariage");
        }
    }
}
