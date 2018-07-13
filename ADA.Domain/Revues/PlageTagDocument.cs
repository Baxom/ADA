using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Revues
{
    public abstract class PlageTagDocument : EntityBase
    {
        public ListePlage Pages { get; set; }

        public abstract string Tag { get; }

        public abstract string ShortTag { get; }

        public abstract IEnumerable<Document> GetDocuments();
    }
}
