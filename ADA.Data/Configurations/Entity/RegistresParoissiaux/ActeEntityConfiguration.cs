using ADA.Domain.Pretres;
using ADA.Domain.RegistresParoissiaux;
using ADA.Domain.Revues;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Data.Configurations
{
    public class ActeEntityConfiguration : EntityTypeConfiguration<Acte>
    {
        public ActeEntityConfiguration()
        {
            this.Property(b => b.Pages.ListePagesTexte).HasColumnName("Pages");
          
            this.Map<Mariage>(m => {

                m.Property(b => b.DateMariageCivil);
                m.Property(b => b.DateMariageReligieux);
                
                m.Property(b => b.Epoux.Nom).HasColumnName("NomEpoux");
                m.Property(b => b.Epoux.Prenom).HasColumnName("PrenomEpoux");
                m.Property(b => b.Epouse.Nom).HasColumnName("NomEpouse");
                m.Property(b => b.Epouse.Prenom).HasColumnName("PrenomEpouse");
                m.MapInheritedProperties();
                m.ToTable("Mariage");
            });
            
        }

    }

    public class ActeIndividuelEntityConfiguration : EntityTypeConfiguration<ActeIndividuel>
    {
        public ActeIndividuelEntityConfiguration()
        {
            this.Property(b => b.Nom).IsRequired();
            this.Property(b => b.Prenom).IsRequired();


            this.Map<Bapteme>(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Bapteme");
            });
          
            this.Map<Sepulture>(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("Sepulture");
            });

        }

    }



}
