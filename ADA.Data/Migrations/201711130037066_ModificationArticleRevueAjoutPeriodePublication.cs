namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModificationArticleRevueAjoutPeriodePublication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ArticleRevue", "DebutPublication", c => c.Int());
            AddColumn("dbo.ArticleRevue", "FinPublication", c => c.Int());
            AddColumn("dbo.PretreArticleRevue", "DebutPublication", c => c.Int());
            AddColumn("dbo.PretreArticleRevue", "FinPublication", c => c.Int());

            Sql(@"ALTER TABLE dbo.PretreArticleRevue ADD PeriodePublication AS (
                CASE 
                    WHEN ISNULL(DebutPublication,FinPublication) IS NULL THEN NULL
                    WHEN DebutPublication IS NULL OR FinPublication Is NULL THEN CONVERT(varchar(9), ISNULL(DebutPublication,FinPublication))
                    ELSE (CASE WHEN DebutPublication = FinPublication THEN CONVERT(varchar(9), DebutPublication )
                            ELSE CONVERT(varchar(4), DebutPublication) + '-' + CONVERT(varchar(4), FinPublication) END)
                END
                )");


            Sql(@"ALTER TABLE dbo.ArticleRevue ADD PeriodePublication AS (
                CASE 
                    WHEN ISNULL(DebutPublication,FinPublication) IS NULL THEN NULL
                    WHEN DebutPublication IS NULL OR FinPublication Is NULL THEN CONVERT(varchar(9), ISNULL(DebutPublication,FinPublication))
                    ELSE (CASE WHEN DebutPublication = FinPublication THEN CONVERT(varchar(9), DebutPublication )
                            ELSE CONVERT(varchar(4), DebutPublication) + '-' + CONVERT(varchar(4), FinPublication) END)
                END
                )");

            DropColumn("dbo.ArticleRevue", "Annee");
            DropColumn("dbo.PretreArticleRevue", "Annee");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PretreArticleRevue", "Annee", c => c.String());
            AddColumn("dbo.ArticleRevue", "Annee", c => c.String());
            DropColumn("dbo.PretreArticleRevue", "PeriodePublication");
            DropColumn("dbo.PretreArticleRevue", "FinPublication");
            DropColumn("dbo.PretreArticleRevue", "DebutPublication");
            DropColumn("dbo.ArticleRevue", "PeriodePublication");
            DropColumn("dbo.ArticleRevue", "FinPublication");
            DropColumn("dbo.ArticleRevue", "DebutPublication");
        }
    }
}
