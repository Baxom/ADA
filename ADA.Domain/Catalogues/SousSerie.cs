using ADA.Domain.Base;
using ADA.Domain.Indexation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Catalogues
{
    public class SousSerie : SerieBase
    {
        public int SerieId { get; set; }
        public Serie Serie { get; set; }
    }
}
