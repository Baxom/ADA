using ADA.Domain.Base;
using ADA.Domain.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Fonds
{
    public class InformationFiltreInjectionFond : EntityBase
    {
        public InformationRechercheFond InformationRechercheFond { get; set; }
        public InformationFond InformationFond { get; set; }
        public TypeColonneOperateur Operateur { get; set; }
        
    }
}
