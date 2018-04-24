using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Lieux
{
    public class Lieu : EntityBase
    {
        public TypeLieu TypeLieu { get; set; }
        public String Nom { get; set; }
    }
}
