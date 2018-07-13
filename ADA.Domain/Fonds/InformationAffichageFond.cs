using ADA.Domain.Base;
using ADA.Domain.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Fonds
{
    public class InformationAffichageFond : EntityBase
    {
        public string Libelle { get; set; }
        public string Pattern { get; set; }
        public int Ordre { get; set; }

        public string BuildPattern(List<InformationFondMedium> infos)
        {
            string resultat = Pattern;

            infos.ForEach(b => resultat = resultat.Replace(b.InformationFond.Code, b.GetStringValue()));

            return resultat;
        }
    }
}
