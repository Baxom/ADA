namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeCodeInseeType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Commune", "CodeInsee", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Commune", "CodeInsee", c => c.Int(nullable: false));
        }
    }
}
