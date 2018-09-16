namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifCatalogueTitreType : DbMigration
    {
        public override void Up()
        {
            Sql("AlTER TABLE CATALOGUE ALTER COLUMN TITRE Nvarchar(max)");
        }
        
        public override void Down()
        {
            Sql("AlTER TABLE CATALOGUE ALTER COLUMN TITRE NTeXT");
        }
    }
}
