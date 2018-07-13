using ADA.Domain.Fonds;
using ADA.Domain.Media;
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
    public class MediumEntityConfiguration : EntityTypeConfiguration<Medium>
    {
        public MediumEntityConfiguration()
        {          
            this.Map<FondMedium>(m => {
                
                m.MapInheritedProperties();
                m.ToTable("FondMedium");
            });
            
        }

    }



}
