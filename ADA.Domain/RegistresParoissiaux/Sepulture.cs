using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.RegistresParoissiaux
{
    public class Sepulture : ActeIndividuel
    {
        public DateTime? DateDeces { get; set; }
        public DateTime? DateSepulture { get; set; }
    }
}

