namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutSousDossierRevue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Revue", "NomDossier", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Revue", "NomDossier");
        }
    }
}
