namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutCatalogue1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Catalogue", "SousSerieId", "dbo.Index");
            DropIndex("dbo.Catalogue", new[] { "SousSerieId" });
            DropColumn("dbo.Catalogue", "SousSerieId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Catalogue", "SousSerieId", c => c.Int());
            CreateIndex("dbo.Catalogue", "SousSerieId");
            AddForeignKey("dbo.Catalogue", "SousSerieId", "dbo.Index", "Id");
        }
    }
}
