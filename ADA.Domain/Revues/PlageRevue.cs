using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;

namespace ADA.Domain.Revues
{
    public class PlageRevue : PlageTagDocument
    {
        public int? DebutPublication { get; set; }
        public int? FinPublication { get; set; }
        public string PeriodePublication { get; private set; }
        public int PremierePage { get; private set; }
        public int? NumeroRevue { get; set; }
        public Revue Revue { get; set; }

        public override string Tag { 
            get {
                return Revue.BuildTag(0, PeriodePublication, NumeroRevue);
            } 
        }

        public override string ShortTag
        {
            get
            {
                return String.Format("{0}, année {1}", Revue.Code, PeriodePublication);
            }
        }

        public override IEnumerable<Document> GetDocuments()
        {
            return Pages.ListePages.Select(b => new DocumentRevue(this.Tag, Revue.BuildNomCompletFichier(b, PeriodePublication, NumeroRevue))).DistinctBy( b => b.NomCompletFichier );
        }
    }
}
