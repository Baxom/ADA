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
    public class PretreEntityConfiguration : EntityTypeConfiguration<Pretre>
    {
        public PretreEntityConfiguration()
        {
            this.ToTable("Pretre");


            this.HasMany(b => b.FonctionsLieu).WithRequired();
            this.HasMany(b => b.Documents).WithRequired();
            this.HasMany(b => b.ArticlesRevue).WithRequired();
            this.HasMany(b => b.Photos).WithRequired();
            

                        
        }
        
    }
}
