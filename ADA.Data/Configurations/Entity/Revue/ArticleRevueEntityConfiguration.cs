using ADA.Domain.Pretres;
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
    public class ArticleRevueEntityConfiguration : EntityTypeConfiguration<ArticleRevue>
    {
        public ArticleRevueEntityConfiguration()
        {
            this.HasMany(b => b.ArticleRevueIndex).WithRequired();
            this.Property(b => b.PagesReelles.ListePagesTexte).HasColumnName("PagesReelles");
            this.Property(b => b.PagesVirtuelles.ListePagesTexte).HasColumnName("PagesAffichees");
            this.Property(b => b.PeriodePublication).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.HasRequired(b => b.Revue);
        }
        
    }
}
