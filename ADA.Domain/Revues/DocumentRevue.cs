using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Revues
{
    public class DocumentRevue : Document
    {
       
        public DocumentRevue(string tag, string nomFichier)
            : base(tag, nomFichier, System.Configuration.ConfigurationManager.AppSettings["RepertoireRevue"])
        {
            
        }
    }
}
