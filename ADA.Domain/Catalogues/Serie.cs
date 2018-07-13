using ADA.Domain.Base;
using ADA.Domain.Indexation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Catalogues
{
    public class Serie : SerieBase
    {
        public virtual ICollection<SousSerie> SousSeries { get; set; }
    }
}
