namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreationCatalogueFTS : DbMigration
    {
        public override void Up()
        {
            Sql(@"CREATE FULLTEXT CATALOG [CatalogueFts] WITH ACCENT_SENSITIVITY = OFF AS DEFAULT", true);
        }

        public override void Down()
        {
            Sql(@"DROP FULLTEXT CATALOG [CatalogueFts]", true);
        }
    }
}
