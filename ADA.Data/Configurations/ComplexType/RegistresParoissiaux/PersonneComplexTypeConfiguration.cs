using ADA.Domain.RegistresParoissiaux;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Data.Configurations.ComplexType.RegistresParoissiaux
{
    public class PersonneComplexTypeConfiguration : ComplexTypeConfiguration<Personne>
    {
        public PersonneComplexTypeConfiguration()
        {
         /*   this.Property(b => b.Nom).HasColumnName("Personne_Nom");
            this.Property(b => b.Prenom).HasColumnName("Personne_Prenom");*/
        }

    }
}
