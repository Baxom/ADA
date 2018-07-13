using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.Domain.References;
using ADA.Domain.Constantes;
using ADA.Domain.Media;

namespace ADA.Domain.Fonds
{
    public class FondMedium : Medium
    {
        public virtual Fond Fond { get; set; }
        public ICollection<InformationFondMedium> ColonneFondMedium { get; set; }
        public ICollection<FondMediumIndex> FondMediumIndex { get; set; }

        public Dictionary<string, string> GetInformationsAffichage()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            this.Fond.InformationAffichageFonds.ForEach(iaf =>
            {
                result.Add(iaf.Libelle, iaf.BuildPattern(ColonneFondMedium.ToList()));
            });

            return result;
        }
    }

}

