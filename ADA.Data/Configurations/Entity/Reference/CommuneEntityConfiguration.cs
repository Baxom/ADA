using ADA.Domain.Pretres;
using ADA.Domain.References;
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
    public class CommuneEntityConfiguration : EntityTypeConfiguration<Commune>
    {
        public CommuneEntityConfiguration()
        {
            this.HasKey(b => b.CodeInsee);
        }

    }
    
}
