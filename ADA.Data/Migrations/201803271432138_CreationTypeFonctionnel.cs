namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreationTypeFonctionnel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TypeLieu", "TypeFonctionnel", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TypeLieu", "TypeFonctionnel");
        }
    }
}
