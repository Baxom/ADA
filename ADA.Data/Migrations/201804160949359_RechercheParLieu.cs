namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RechercheParLieu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TypeLieu", "RechercheParLieu", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TypeLieu", "RechercheParLieu");
        }
    }
}
