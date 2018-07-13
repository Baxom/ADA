using ADA.Domain.Base;
using ADA.Domain.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Fonds
{
    public class Fond : EntityBase
    {
        public string Code { get; set; }
        public string Nom { get; set; }
        public string Repertoire { get; set; }

        public bool Actif { get; set; }

        public string Description { get; set; }

        public TypeVueFond TypeVue { get; set; }

        public Fond FondPere { get; set; }
        public ICollection<Fond> FondsFils { get; set; }

        public ICollection<FondGroupeIndex> FondGroupeIndex { get; set; }
        public List<InformationRechercheFond> InformationRechercheFonds { get; set; }
        public List<InformationFond> InformationFonds { get; set; }
        public List<InformationAffichageFond> InformationAffichageFonds { get; set; }
    }
}
