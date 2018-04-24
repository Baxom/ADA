using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.Domain.Indexation;

namespace ADA.Domain.Revues
{
    public class RevueGroupeIndex : EntityBase
    {
        public GroupeIndex GroupeIndex { get; set; }
    }
}
