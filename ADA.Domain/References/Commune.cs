using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.References
{
    public class Commune : EntityBase
    {
        public string CodeInsee { get; set; }
        public string Nom { get; set; }
        public string CodePostal { get; set; }
    }
}
