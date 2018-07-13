namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjoutTypeVUeFond : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Fond", "TypeVue", c => c.Int(nullable: false, defaultValue: (int)Domain.Constantes.TypeVueFond.DefautMedia));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Fond", "TypeVue");
        }
    }
}
