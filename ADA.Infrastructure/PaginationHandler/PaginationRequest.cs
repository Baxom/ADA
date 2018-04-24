using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Infrastructure.PaginationHandler
{
    public class PaginationRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginationRequest(int pageSize, int pageNumber)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class PaginationRequest<T> : PaginationRequest
    {
        public T Parameters { get; set; }

        public PaginationRequest(int pageSize, int rowNumber, T parameters) : base(pageSize, rowNumber)
        {
            Parameters = parameters;
        }
    }
}
