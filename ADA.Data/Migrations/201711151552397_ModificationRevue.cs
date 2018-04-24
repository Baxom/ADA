namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificationRevue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Revue", "TagModel", c => c.String());
            AddColumn("dbo.Revue", "Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Revue", "RechercheDirecte", c => c.Boolean(nullable: false));
            DropColumn("dbo.Revue", "NomDossier");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Revue", "NomDossier", c => c.String());
            DropColumn("dbo.Revue", "RechercheDirecte");
            DropColumn("dbo.Revue", "Active");
            DropColumn("dbo.Revue", "TagModel");
        }
    }
}
