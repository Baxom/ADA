namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutFonctionParLieuEtPartypeLieu : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FonctionLieu",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        FonctionId = c.Int(),
                        LieuId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fonction", t => t.FonctionId)
                .ForeignKey("dbo.Lieu", t => t.LieuId)
                .Index(t => t.FonctionId)
                .Index(t => t.LieuId);
            
            CreateTable(
                "dbo.FonctionTypeLieu",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        FonctionId = c.Int(),
                        TypeLieuId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fonction", t => t.FonctionId)
                .ForeignKey("dbo.TypeLieu", t => t.TypeLieuId)
                .Index(t => t.FonctionId)
                .Index(t => t.TypeLieuId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FonctionTypeLieu", "TypeLieuId", "dbo.TypeLieu");
            DropForeignKey("dbo.FonctionTypeLieu", "FonctionId", "dbo.Fonction");
            DropForeignKey("dbo.FonctionLieu", "LieuId", "dbo.Lieu");
            DropForeignKey("dbo.FonctionLieu", "FonctionId", "dbo.Fonction");
            DropIndex("dbo.FonctionTypeLieu", new[] { "TypeLieuId" });
            DropIndex("dbo.FonctionTypeLieu", new[] { "FonctionId" });
            DropIndex("dbo.FonctionLieu", new[] { "LieuId" });
            DropIndex("dbo.FonctionLieu", new[] { "FonctionId" });
            DropTable("dbo.FonctionTypeLieu");
            DropTable("dbo.FonctionLieu");
        }
    }
}
