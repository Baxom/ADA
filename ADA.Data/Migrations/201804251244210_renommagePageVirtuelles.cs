namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class renommagePageVirtuelles : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.ArticleRevue", "PagesAffichees", "PagesReferences");
            RenameColumn("dbo.ArticleRevue", "PagesReelles", "Pages");

        }

        public override void Down()
        {
            RenameColumn("dbo.ArticleRevue", "PagesReferences", "PagesAffichees");
            RenameColumn("dbo.ArticleRevue", "Pages", "PagesReelles");

        }
    }
}
