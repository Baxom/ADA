namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutNumeroRevue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleRevue", "NumeroRevue", c => c.Int());
            AddColumn("dbo.PretreArticleRevue", "NumeroRevue", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PretreArticleRevue", "NumeroRevue");
            DropColumn("dbo.ArticleRevue", "NumeroRevue");
        }
    }
}
