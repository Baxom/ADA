using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Services.Interface
{
    public interface IPretreService
    {
        void CreatePdf(int pretreId, Stream memoryStream);
        void CreatePdf(IEnumerable<int> pretreIds, Stream memoryStream);
    }
}
