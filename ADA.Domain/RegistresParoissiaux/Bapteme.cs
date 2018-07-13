using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.RegistresParoissiaux
{
    public class Bapteme : ActeIndividuel
    {
        public DateTime? DateNaissance { get; set; }
        public DateTime? DateBapteme { get; set; }

        public override string Libelle { get { return "Baptême"; } }
        public override string LibellePluriel { get { return "Baptêmes"; } }
    }
}
