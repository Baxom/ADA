namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifCleEtrangere : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ArticleRevueIndex", "ArticleRevueId", "dbo.ArticleRevue");
            DropIndex("dbo.ArticleRevueIndex", new[] { "ArticleRevueId" });
            AlterColumn("dbo.ArticleRevueIndex", "ArticleRevueId", c => c.Int(nullable: false));
            CreateIndex("dbo.ArticleRevueIndex", "ArticleRevueId");
            AddForeignKey("dbo.ArticleRevueIndex", "ArticleRevueId", "dbo.ArticleRevue", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArticleRevueIndex", "ArticleRevueId", "dbo.ArticleRevue");
            DropIndex("dbo.ArticleRevueIndex", new[] { "ArticleRevueId" });
            AlterColumn("dbo.ArticleRevueIndex", "ArticleRevueId", c => c.Int());
            CreateIndex("dbo.ArticleRevueIndex", "ArticleRevueId");
            AddForeignKey("dbo.ArticleRevueIndex", "ArticleRevueId", "dbo.ArticleRevue", "Id");
        }
    }
}
