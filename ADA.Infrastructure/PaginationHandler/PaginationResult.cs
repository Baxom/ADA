using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Infrastructure.PaginationHandler
{
    public class PaginationResult<T> where T : System.Collections.IEnumerable
    {
        public int CountResult { get; set; }
        public T Data { get; set; }

        public PaginationResult(int countResult, T data)
        {
            CountResult = countResult;
            Data = data;
        }
    }
}
