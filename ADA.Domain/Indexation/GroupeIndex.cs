using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Indexation
{
    public class GroupeIndex : EntityBase
    {
        public string Nom { get; set; }
        public string Libelle { get; set; }
        public ICollection<Index> Index { get; set; }
    }
}
