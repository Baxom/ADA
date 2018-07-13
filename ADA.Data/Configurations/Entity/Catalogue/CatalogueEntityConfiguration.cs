using ADA.Domain.Pretres;
using ADA.Domain.Revues;
using ADA.Domain.Indexation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.Domain.Catalogues;

namespace ADA.Data.Configurations
{
    public class CatalogueEntityConfiguration : EntityTypeConfiguration<Catalogue>
    {
        public CatalogueEntityConfiguration()
        {
            this.Property(b => b.Titre).HasColumnType("ntext");
        }
        
    }
}
