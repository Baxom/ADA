using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.Domain.Indexation;
using ADA.Domain.Base;

namespace ADA.Domain.Revues
{
    public class ArticleRevueIndex : EntityBase
    {
        public Index Index { get; set; }
    }
}
