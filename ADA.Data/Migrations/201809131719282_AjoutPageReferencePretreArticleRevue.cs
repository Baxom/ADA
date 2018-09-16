namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutPageReferencePretreArticleRevue : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PretreArticleRevue", "PagesReferences", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PretreArticleRevue", "PagesReferences");
        }
    }
}
