namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifSousSerieCatalogue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SousSerie", "Code", c => c.String());
            AddColumn("dbo.SousSerie", "Nom", c => c.String());
            AddColumn("dbo.Serie", "Code", c => c.String());
            AddColumn("dbo.Serie", "Nom", c => c.String());
            DropColumn("dbo.SousSerie", "Titre");
            DropColumn("dbo.Serie", "Titre");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Serie", "Titre", c => c.String());
            AddColumn("dbo.SousSerie", "Titre", c => c.String());
            DropColumn("dbo.Serie", "Nom");
            DropColumn("dbo.Serie", "Code");
            DropColumn("dbo.SousSerie", "Nom");
            DropColumn("dbo.SousSerie", "Code");
        }
    }
}
