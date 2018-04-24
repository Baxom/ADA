using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Revues
{
    public class PlageRevue : EntityBase
    {
        public ListePlage PagesReelles { get; set; }
        public int? DebutPublication { get; set; }
        public int? FinPublication { get; set; }
        public string PeriodePublication { get; private set; }
        public Revue Revue { get; set; }

        public string Tag { 
            get {
                return Revue.BuildTag(0, PeriodePublication);
            } 
        }

        public string ShortTag
        {
            get
            {
                return String.Format("{0}, année {1}", Revue.Code, PeriodePublication);
            }
        }

        public IEnumerable<DocumentRevue> GetDocuments()
        {
            return PagesReelles.Plages.SelectMany(b => b.Pages).Select(b => new DocumentRevue(this.Tag, Revue.BuildNomCompletFichier(b, PeriodePublication)));
        }
    }
}
