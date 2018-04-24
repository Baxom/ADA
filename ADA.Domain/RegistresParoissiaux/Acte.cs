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
    public abstract class Acte : EntityBase
    {
        public int AnneeRegistreParoissial { get; set; }
        public int ParoisseRegistreId { get; set; }
        public Lieu ParoisseRegistre { get; set; }
        public ListePlage Pages { get; set; }
    }
}
