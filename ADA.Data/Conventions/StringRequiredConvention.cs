using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Data.Conventions
{
    public class StringRequiredConvention : Convention
    {
        public StringRequiredConvention()
        {
            string[] requiredString = new string[] { "Code", "Nom" };

            this.Properties()
                .Where(b => requiredString.Contains(b.Name))
                .Configure(b => b.IsRequired());

            this.Properties()
                .Where(b => b.Name == "Nom" && b.PropertyType == typeof(string))
                .Configure(b => b.HasMaxLength(150));

            this.Properties()
               .Where(b => b.Name == "Code" && b.PropertyType == typeof(string))
               .Configure(b => b.HasMaxLength(50));

            this.Properties()
               .Where(b => b.Name == "CodeExterne" && b.PropertyType == typeof(string))
               .Configure(b => b.HasMaxLength(50));
              
        }
    }
}
