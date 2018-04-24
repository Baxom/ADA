namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutPageAffichées : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleRevue", "PagesAffichees", c => c.String());
            RenameColumn("dbo.ArticleRevue", "Pages", "PagesReelles");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ArticleRevue", "Pages", c => c.String());
            RenameColumn("dbo.ArticleRevue", "PagesReelles", "Pages");
        }
    }
}
