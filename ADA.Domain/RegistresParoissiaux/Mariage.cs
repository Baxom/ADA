using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.RegistresParoissiaux
{
    public class Mariage : Acte
    {
        public DateTime? DateMariageReligieux { get; set; }
        public DateTime? DateMariageCivil { get; set; }

        public Personne Epoux { get; set; }
        public Personne Epouse { get; set; }
    }
}

