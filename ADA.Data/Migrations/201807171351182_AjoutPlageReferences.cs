namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutPlageReferences : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bapteme", "PagesReferences_ListePagesTexte", c => c.String());
            AddColumn("dbo.Sepulture", "PagesReferences_ListePagesTexte", c => c.String());
            AddColumn("dbo.Mariage", "PagesReferences_ListePagesTexte", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Mariage", "PagesReferences_ListePagesTexte");
            DropColumn("dbo.Sepulture", "PagesReferences_ListePagesTexte");
            DropColumn("dbo.Bapteme", "PagesReferences_ListePagesTexte");
        }
    }
}
