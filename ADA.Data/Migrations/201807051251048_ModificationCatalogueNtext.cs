namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificationCatalogueNtext : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Catalogue", "Titre", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Catalogue", "Titre", c => c.String());
        }
    }
}
