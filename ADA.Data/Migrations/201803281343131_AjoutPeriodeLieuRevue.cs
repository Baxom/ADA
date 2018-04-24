namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutPeriodeLieuRevue : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PeriodesRevueLieu",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        AnneeDebutPeriode = c.Int(nullable: false),
                        AnneeFinPeriode = c.Int(nullable: false),
                        LieuId = c.Int(),
                        RevueId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Lieu", t => t.LieuId)
                .ForeignKey("dbo.Revue", t => t.RevueId)
                .Index(t => t.LieuId)
                .Index(t => t.RevueId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PeriodesRevueLieu", "RevueId", "dbo.Revue");
            DropForeignKey("dbo.PeriodesRevueLieu", "LieuId", "dbo.Lieu");
            DropIndex("dbo.PeriodesRevueLieu", new[] { "RevueId" });
            DropIndex("dbo.PeriodesRevueLieu", new[] { "LieuId" });
            DropTable("dbo.PeriodesRevueLieu");
        }
    }
}
