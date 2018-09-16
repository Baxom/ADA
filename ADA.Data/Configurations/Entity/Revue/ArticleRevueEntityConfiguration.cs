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
            this.Property(b => b.Pages.ListePagesTexte).HasColumnName("Pages");
            this.Property(b => b.PagesReferences.ListePagesTexte).HasColumnName("PagesReferences");
            this.Property(b => b.PeriodePublication).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.Property(b => b.PremierePage).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            this.HasRequired(b => b.Revue);
        }
        
    }
}
