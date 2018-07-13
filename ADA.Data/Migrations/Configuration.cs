namespace ADA.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ADA.Data.Context.ADAContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ADA.Data.Context.ADAContext context)
        {
            InitialisationProcedureStockee(context);
        }

        private void InitialisationProcedureStockee(ADA.Data.Context.ADAContext context)
        {

            var baseDirectory = String.Empty;

            if (String.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath))
            {
                baseDirectory = AppDomain.CurrentDomain.BaseDirectory; //exe folder for WinForms, Consoles, Windows Services
            }
            else
            {
                baseDirectory = AppDomain.CurrentDomain.RelativeSearchPath; //bin folder for Web Apps 
            }

            // Delete all stored procs, views
            foreach (var file in Directory.GetFiles(Path.Combine(baseDirectory, "Sql\\Seed"), "*.sql"))
            {
                context.Database.ExecuteSqlCommand(File.ReadAllText(file), new object[0]);
            }

            // Add Stored Procedures
            foreach (var file in Directory.GetFiles(Path.Combine(baseDirectory, "Sql\\ProceduresStockees"), "*.sql"))
            {
                context.Database.ExecuteSqlCommand(File.ReadAllText(file), new object[0]);
            }
        }
    }
}
