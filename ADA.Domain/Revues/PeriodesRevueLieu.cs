using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADA.Domain.Lieux;

namespace ADA.Domain.Revues
{
    public class PeriodesRevueLieu : EntityBase
    {
        public Revue Revue { get; set; }
        public Lieu Lieu { get; set; }

        public int AnneeDebutPeriode { get; set; }
        public int AnneeFinPeriode { get; set; }
    }
}
