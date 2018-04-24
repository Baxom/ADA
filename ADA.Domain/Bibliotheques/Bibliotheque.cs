using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Bibliotheques
{
    public class Bibliotheque : EntityBase
    {
        public string Auteur { get; set; }
        public string Titre { get; set; }
        public string Annee { get; set; }
        public string Aspect { get; set; }
        public string Epi { get; set; }
        public string Zone { get; set; }

        public string Cote { get { return String.Format("{0}{1}",Zone, Epi); } }

        public bool HasAuteur { get { return !String.IsNullOrEmpty(this.Auteur); } }
    }
}
