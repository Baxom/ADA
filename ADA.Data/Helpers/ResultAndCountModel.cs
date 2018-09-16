using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Data.Helpers
{
    public class ResultAndCountModel
    {
        public ReadOnlyCollection<int> Ids { get; private set; }
        public int Count { get; private set; }

        public ResultAndCountModel(List<int> ids, int count)
        {
            Ids = ids.AsReadOnly();
            Count = count;
        }
    }
}
