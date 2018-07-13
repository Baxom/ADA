using ADA.Domain.Base;
using ADA.Domain.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Fonds
{
    public class InformationRechercheFond : EntityBase
    {
        public string Code { get; set; }
        public string Libelle { get; set; }
        public TypeColonneFond Type { get; set; }
        public bool AutoCompletion { get; set; }
        public int Ordre { get; set; }
        public ICollection<InformationFiltreInjectionFond> InformationFiltreInjectionFonds { get; set; }
    }
}
