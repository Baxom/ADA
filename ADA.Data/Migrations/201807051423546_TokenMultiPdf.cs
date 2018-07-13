namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TokenMultiPdf : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TokenMultiPdf",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Token = c.String(),
                        SerializedIds = c.String(storeType: "ntext"),
                        DatePeremption = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TokenMultiPdf");
        }
    }
}
