using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Revues
{
    public class DocumentActe : Document
    {
       
        public DocumentActe(string tag, string nomFichier)
            : base(tag, nomFichier, System.Configuration.ConfigurationManager.AppSettings["RepertoireRegistre"])
        {
            
        }
    }
}
