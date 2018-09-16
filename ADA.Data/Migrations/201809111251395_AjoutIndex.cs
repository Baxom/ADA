namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutIndex : DbMigration
    {
        public override void Up()
        {
            Sql("CREATE NONCLUSTERED INDEX ix_mediumtag_mediumid ON [dbo].[MediumTag]([Medium_Id])");

        }

        public override void Down()
        {
            Sql("DROP INDEX ix_mediumtag_mediumid ON [dbo].[MediumTag] ");
        }
    }
}
