using ADA.Domain.Base;
using ADA.Domain.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Fonds
{
    public class InformationFond : EntityBase
    {
        public string Nom { get; set; }
        public TypeColonneFond Type { get; set; }
        public string Code { get; set; }
    }
}
