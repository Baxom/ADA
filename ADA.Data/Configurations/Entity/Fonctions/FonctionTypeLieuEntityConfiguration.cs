using ADA.Domain.Fonctions;
using ADA.Domain.Pretres;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Data.Configurations
{
    public class FonctionTypeLieuEntityConfiguration : EntityTypeConfiguration<FonctionTypeLieu>
    {
        public FonctionTypeLieuEntityConfiguration()
        {
            this.HasRequired(b => b.TypeLieu);
            this.HasRequired(b => b.Fonction);
        }
        
    }
}
