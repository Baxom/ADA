namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutRevueVirtuelle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Revue", "RevueMereId", c => c.Int());
            CreateIndex("dbo.Revue", "RevueMereId");
            AddForeignKey("dbo.Revue", "RevueMereId", "dbo.Revue", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Revue", "RevueMereId", "dbo.Revue");
            DropIndex("dbo.Revue", new[] { "RevueMereId" });
            DropColumn("dbo.Revue", "RevueMereId");
        }
    }
}
