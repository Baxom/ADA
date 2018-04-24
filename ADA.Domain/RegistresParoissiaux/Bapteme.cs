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
    }
}
