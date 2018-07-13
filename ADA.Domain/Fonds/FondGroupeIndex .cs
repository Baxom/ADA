using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.Domain.Indexation;

namespace ADA.Domain.Fonds
{
    public class FondGroupeIndex : EntityBase
    {
        public GroupeIndex GroupeIndex { get; set; }
    }
}
