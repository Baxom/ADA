namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutCodeFond : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fond", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fond", "Code");
        }
    }
}
