using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.Domain.Indexation;
using ADA.Domain.Base;

namespace ADA.Domain.Fonds
{
    public class FondMediumIndex : EntityBase
    {
        public Index Index { get; set; }
    }
}
