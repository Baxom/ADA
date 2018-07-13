using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Interne
{
    public class TokenMultiPdf : EntityBase
    {
        public string Token { get; set; }
        public string SerializedIds { get; set; }
        public DateTime DatePeremption { get; set; }

        public List<int> Ids { get; set; }
    }
}
