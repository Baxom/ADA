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
using ADA.Domain.Fonds;

namespace ADA.Data.Configurations
{
    public class FondEntityConfiguration : EntityTypeConfiguration<Fond>
    {
        public FondEntityConfiguration()
        {
            this.Property(b => b.Description).HasColumnType("ntext");
        }
        
    }
}
