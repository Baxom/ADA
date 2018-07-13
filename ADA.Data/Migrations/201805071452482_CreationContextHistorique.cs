namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreationContextHistorique : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PretreContextHistorique",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ContextHistoriqueId = c.Int(),
                        PretreId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ContextHistorique", t => t.ContextHistoriqueId)
                .ForeignKey("dbo.Pretre", t => t.PretreId)
                .Index(t => t.ContextHistoriqueId)
                .Index(t => t.PretreId);
            
            CreateTable(
                "dbo.ContextHistorique",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Nom = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropColumn("dbo.TypeLieu", "RechercheParLieu");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TypeLieu", "RechercheParLieu", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.PretreContextHistorique", "PretreId", "dbo.Pretre");
            DropForeignKey("dbo.PretreContextHistorique", "ContextHistoriqueId", "dbo.ContextHistorique");
            DropIndex("dbo.PretreContextHistorique", new[] { "PretreId" });
            DropIndex("dbo.PretreContextHistorique", new[] { "ContextHistoriqueId" });
            DropTable("dbo.ContextHistorique");
            DropTable("dbo.PretreContextHistorique");
        }
    }
}
