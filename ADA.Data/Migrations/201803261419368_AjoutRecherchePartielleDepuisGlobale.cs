namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutRecherchePartielleDepuisGlobale : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Revue", "RecherchePartielleDepuisGlobale", c => c.Boolean(nullable: false));
            AddColumn("dbo.Revue", "RecherchePartielleDepuisGlobaleDefaut", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Revue", "RecherchePartielleDepuisGlobaleDefaut");
            DropColumn("dbo.Revue", "RecherchePartielleDepuisGlobale");
        }
    }
}
