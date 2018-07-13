using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Media
{
    public class MediumTag : EntityBase
    {
        public Medium Medium { get; set; }
        public Tag Tag { get; set; }
    }
}
