namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutInformationFiltreInjection : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InformationFiltreInjectionFond",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Operateur = c.Int(nullable: false),
                        InformationFondId = c.Int(),
                        InformationRechercheFondId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InformationFond", t => t.InformationFondId)
                .ForeignKey("dbo.InformationRechercheFond", t => t.InformationRechercheFondId)
                .Index(t => t.InformationFondId)
                .Index(t => t.InformationRechercheFondId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InformationFiltreInjectionFond", "InformationRechercheFondId", "dbo.InformationRechercheFond");
            DropForeignKey("dbo.InformationFiltreInjectionFond", "InformationFondId", "dbo.InformationFond");
            DropIndex("dbo.InformationFiltreInjectionFond", new[] { "InformationRechercheFondId" });
            DropIndex("dbo.InformationFiltreInjectionFond", new[] { "InformationFondId" });
            DropTable("dbo.InformationFiltreInjectionFond");
        }
    }
}
