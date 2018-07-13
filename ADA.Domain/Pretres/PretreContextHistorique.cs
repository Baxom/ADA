using ADA.Domain.Base;
using ADA.Domain.Contexthistoriques;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Pretres
{
    public class PretreContextHistorique : EntityBase
    {
        public ContextHistorique ContextHistorique { get; set; }
    }
}
