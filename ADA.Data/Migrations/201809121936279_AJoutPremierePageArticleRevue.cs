namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AJoutPremierePageArticleRevue : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TABLE dbo.ArticleRevue ADD PremierePage AS (
                CASE WHEN SUBSTRING([Pages], 0, CASE WHEN PATINDEX('%[,-]%',[Pages]) = 0 THEN 10 ELSE PATINDEX('%[,-]%',[Pages]) END) NOT LIKE '%[^0-9]%' THEN 
	              CAST(SUBSTRING([Pages], 0, CASE WHEN PATINDEX('%[,-]%',[Pages]) = 0 THEN 10 ELSE PATINDEX('%[,-]%',[Pages]) END) as int) 
	              ELSE 0 END
                ) PERSISTED");
        }
        
        public override void Down()
        {
            DropColumn("dbo.ArticleRevue", "PremierePage");
        }
    }
}
