namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutIndexEtGroupeIndex : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleRevueIndex",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        IndexId = c.Int(),
                        ArticleRevueId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Index", t => t.IndexId)
                .ForeignKey("dbo.ArticleRevue", t => t.ArticleRevueId)
                .Index(t => t.IndexId)
                .Index(t => t.ArticleRevueId);
            
            CreateTable(
                "dbo.Index",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Libelle = c.String(),
                        GroupeIndexId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupeIndex", t => t.GroupeIndexId)
                .Index(t => t.GroupeIndexId);
            
            CreateTable(
                "dbo.RevueGroupeIndex",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        GroupeIndexId = c.Int(),
                        RevueId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GroupeIndex", t => t.GroupeIndexId)
                .ForeignKey("dbo.Revue", t => t.RevueId)
                .Index(t => t.GroupeIndexId)
                .Index(t => t.RevueId);
            
            CreateTable(
                "dbo.GroupeIndex",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Nom = c.String(),
                        Libelle = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Revue", "RechercheParAnnee", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RevueGroupeIndex", "RevueId", "dbo.Revue");
            DropForeignKey("dbo.RevueGroupeIndex", "GroupeIndexId", "dbo.GroupeIndex");
            DropForeignKey("dbo.Index", "GroupeIndexId", "dbo.GroupeIndex");
            DropForeignKey("dbo.ArticleRevueIndex", "ArticleRevueId", "dbo.ArticleRevue");
            DropForeignKey("dbo.ArticleRevueIndex", "IndexId", "dbo.Index");
            DropIndex("dbo.RevueGroupeIndex", new[] { "RevueId" });
            DropIndex("dbo.RevueGroupeIndex", new[] { "GroupeIndexId" });
            DropIndex("dbo.Index", new[] { "GroupeIndexId" });
            DropIndex("dbo.ArticleRevueIndex", new[] { "ArticleRevueId" });
            DropIndex("dbo.ArticleRevueIndex", new[] { "IndexId" });
            DropColumn("dbo.Revue", "RechercheParAnnee");
            DropTable("dbo.GroupeIndex");
            DropTable("dbo.RevueGroupeIndex");
            DropTable("dbo.Index");
            DropTable("dbo.ArticleRevueIndex");
        }
    }
}
