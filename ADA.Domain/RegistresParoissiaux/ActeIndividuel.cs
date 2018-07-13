using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.Domain.Revues;
using ADA.Domain.Lieux;

namespace ADA.Domain.RegistresParoissiaux
{
    public abstract class ActeIndividuel : Acte
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string NomPrenom { get { return String.Format("{0} {1}", this.Nom, this.Prenom); } }
        public override string Denomination { get { return NomPrenom; } }
    }
}
