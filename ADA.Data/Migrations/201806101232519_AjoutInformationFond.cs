namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutInformationFond : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MediumTag", "MediumId", "dbo.Medium");
            DropIndex("dbo.MediumTag", new[] { "MediumId" });
            CreateTable(
                "dbo.InformationFondMedium",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ValueString = c.String(),
                        ValueInt = c.Int(),
                        ValueDate = c.DateTime(),
                        InformationFondId = c.Int(),
                        FondMediumId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InformationFond", t => t.InformationFondId)
                .ForeignKey("dbo.FondMedium", t => t.FondMediumId)
                .Index(t => t.InformationFondId)
                .Index(t => t.FondMediumId);
            
            CreateTable(
                "dbo.InformationFond",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Nom = c.String(),
                        Type = c.Int(nullable: false),
                        Code = c.String(),
                        FondId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fond", t => t.FondId)
                .Index(t => t.FondId);
            
            CreateTable(
                "dbo.Fond",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Nom = c.String(),
                        Repertoire = c.String(),
                        Actif = c.Boolean(nullable: false),
                        FondPereId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fond", t => t.FondPereId)
                .Index(t => t.FondPereId);
            
            CreateTable(
                "dbo.FondGroupeIndex",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        GroupeIndexId = c.Int(),
                        FondId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupeIndex", t => t.GroupeIndexId)
                .ForeignKey("dbo.Fond", t => t.FondId)
                .Index(t => t.GroupeIndexId)
                .Index(t => t.FondId);
            
            CreateTable(
                "dbo.InformationAffichageFond",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Libelle = c.String(),
                        Pattern = c.String(),
                        Ordre = c.Int(nullable: false),
                        FondId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fond", t => t.FondId)
                .Index(t => t.FondId);
            
            CreateTable(
                "dbo.InformationRechercheFond",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Code = c.String(),
                        Libelle = c.String(),
                        Type = c.Int(nullable: false),
                        AutoCompletion = c.Boolean(nullable: false),
                        Ordre = c.Int(nullable: false),
                        FondId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fond", t => t.FondId)
                .Index(t => t.FondId);
            
            CreateTable(
                "dbo.FondMediumIndex",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        IndexId = c.Int(),
                        FondMediumId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Index", t => t.IndexId)
                .ForeignKey("dbo.FondMedium", t => t.FondMediumId)
                .Index(t => t.IndexId)
                .Index(t => t.FondMediumId);
            
            CreateTable(
                "dbo.FondMedium",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        FondId = c.Int(),
                        Titre = c.String(),
                        Type = c.Int(nullable: false),
                        TagsString = c.String(),
                        FileName = c.String(),
                        ThumbNail = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fond", t => t.FondId)
                .Index(t => t.FondId);
            
            AddColumn("dbo.MediumTag", "Medium_Id", c => c.Int());
            DropColumn("dbo.MediumTag", "MediumId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MediumTag", "MediumId", c => c.Int());
            DropForeignKey("dbo.FondMedium", "FondId", "dbo.Fond");
            DropForeignKey("dbo.FondMediumIndex", "FondMediumId", "dbo.FondMedium");
            DropForeignKey("dbo.FondMediumIndex", "IndexId", "dbo.Index");
            DropForeignKey("dbo.InformationRechercheFond", "FondId", "dbo.Fond");
            DropForeignKey("dbo.InformationFond", "FondId", "dbo.Fond");
            DropForeignKey("dbo.InformationAffichageFond", "FondId", "dbo.Fond");
            DropForeignKey("dbo.Fond", "FondPereId", "dbo.Fond");
            DropForeignKey("dbo.FondGroupeIndex", "FondId", "dbo.Fond");
            DropForeignKey("dbo.FondGroupeIndex", "GroupeIndexId", "dbo.GroupeIndex");
            DropForeignKey("dbo.InformationFondMedium", "FondMediumId", "dbo.FondMedium");
            DropForeignKey("dbo.InformationFondMedium", "InformationFondId", "dbo.InformationFond");
            DropIndex("dbo.FondMedium", new[] { "FondId" });
            DropIndex("dbo.FondMediumIndex", new[] { "FondMediumId" });
            DropIndex("dbo.FondMediumIndex", new[] { "IndexId" });
            DropIndex("dbo.InformationRechercheFond", new[] { "FondId" });
            DropIndex("dbo.InformationAffichageFond", new[] { "FondId" });
            DropIndex("dbo.FondGroupeIndex", new[] { "FondId" });
            DropIndex("dbo.FondGroupeIndex", new[] { "GroupeIndexId" });
            DropIndex("dbo.Fond", new[] { "FondPereId" });
            DropIndex("dbo.InformationFond", new[] { "FondId" });
            DropIndex("dbo.InformationFondMedium", new[] { "FondMediumId" });
            DropIndex("dbo.InformationFondMedium", new[] { "InformationFondId" });
            DropColumn("dbo.MediumTag", "Medium_Id");
            DropTable("dbo.FondMedium");
            DropTable("dbo.FondMediumIndex");
            DropTable("dbo.InformationRechercheFond");
            DropTable("dbo.InformationAffichageFond");
            DropTable("dbo.FondGroupeIndex");
            DropTable("dbo.Fond");
            DropTable("dbo.InformationFond");
            DropTable("dbo.InformationFondMedium");
            CreateIndex("dbo.MediumTag", "MediumId");
            AddForeignKey("dbo.MediumTag", "MediumId", "dbo.Medium", "Id");
        }
    }
}
