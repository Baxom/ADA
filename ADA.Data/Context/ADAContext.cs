using ADA.Data.Conventions;
using ADA.Domain.Bibliotheques;
using ADA.Domain.Catalogues;
using ADA.Domain.Contexthistoriques;
using ADA.Domain.Fonctions;
using ADA.Domain.Lieux;
using ADA.Domain.Media;
using ADA.Domain.Pretres;
using ADA.Domain.References;
using ADA.Domain.RegistresParoissiaux;
using ADA.Domain.Revues;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Data.Context
{
    public class ADAContext : DbContext
    {
        public ADAContext() :
            base("name=ADAContext")
        {
            Init();

            
        }

        private void Init()
        {
            this.Configuration.LazyLoadingEnabled = true;
            Database
                .SetInitializer<ADAContext>
                (new MigrateDatabaseToLatestVersion<ADAContext, ADA.Data.Migrations.Configuration>(true));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Add<PrimaryKeyConvention>();
            modelBuilder.Conventions.AddBefore<ForeignKeyIndexConvention>(new ForeignKeyNamingConvention());
           // modelBuilder.Conventions.Add<StringRequiredConvention>();
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }

        DbSet<Acte> Actes { get; set; }
        DbSet<ArticleRevue> ArticlesRevue { get; set; }

        DbSet<Bibliotheque> Bibliotheques { get; set; }
        DbSet<Catalogue> Catalogues { get; set; }
        DbSet<Commune> Communes { get; set; }
        DbSet<ContextHistorique> ContextHistoriques { get; set; }

        DbSet<Fonction> Fonctions { get; set; }
        DbSet<FonctionLieu> FonctionsLieu { get; set; }
        DbSet<FonctionTypeLieu> FonctionsTypeLieu { get; set; }
        
        DbSet<Lieu> Lieux { get; set; }

        DbSet<Medium> Media { get; set; }

        DbSet<Pretre> Pretres { get; set; }
        DbSet<PretreFonctionLieu> PretreFonctionsLieu { get; set; }

        DbSet<Revue> Revue { get; set; }

        DbSet<TypeLieu> TypeLieu { get; set; }


    }
}
