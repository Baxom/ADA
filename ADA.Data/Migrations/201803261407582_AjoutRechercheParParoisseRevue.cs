namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutRechercheParParoisseRevue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Revue", "RechercheParParoisse", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Revue", "RechercheParParoisse");
        }
    }
}
