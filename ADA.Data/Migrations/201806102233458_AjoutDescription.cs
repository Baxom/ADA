namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fond", "Description", c => c.String(storeType: "ntext"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fond", "Description");
        }
    }
}
