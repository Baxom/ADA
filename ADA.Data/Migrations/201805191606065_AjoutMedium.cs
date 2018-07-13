namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutMedium : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Medium",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Titre = c.String(),
                        Type = c.Int(nullable: false),
                        TagsString = c.String(),
                        FileName = c.String(),
                        ThumbNail = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MediumTag",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        MediumId = c.Int(),
                        TagId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Medium", t => t.MediumId)
                .ForeignKey("dbo.Tag", t => t.TagId)
                .Index(t => t.MediumId)
                .Index(t => t.TagId);
            
            CreateTable(
                "dbo.Tag",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Libelle = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MediumTag", "TagId", "dbo.Tag");
            DropForeignKey("dbo.MediumTag", "MediumId", "dbo.Medium");
            DropIndex("dbo.MediumTag", new[] { "TagId" });
            DropIndex("dbo.MediumTag", new[] { "MediumId" });
            DropTable("dbo.Tag");
            DropTable("dbo.MediumTag");
            DropTable("dbo.Medium");
        }
    }
}
