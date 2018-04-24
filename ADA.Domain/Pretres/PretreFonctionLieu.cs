using ADA.Domain.Base;
using ADA.Domain.Fonctions;
using ADA.Domain.Lieux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Pretres
{
    public class PretreFonctionLieu : EntityBase
    {
        public int PretreId { get; set; }
        public Lieu Lieu { get; set; }
        public Fonction Fonction { get; set; }
        public int? AnneeDebut { get; set; }
        public int? AnneeFin { get; set; }
    }
}
