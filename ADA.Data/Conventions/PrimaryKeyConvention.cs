using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Data.Conventions
{
    /// <summary>
    /// Provides a convention for fixing the independent association (IA) foreign key column names.
    /// </summary>
    public class PrimaryKeyConvention : Convention
    {

        public PrimaryKeyConvention()
        {
            this.Properties().Where(b => b.Name == "Id").Configure(b => b.HasDatabaseGeneratedOption(DatabaseGeneratedOption.None));
        }

    }
}
