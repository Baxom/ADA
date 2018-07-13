using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Services.Interface
{
    public interface IActeService
    {
        void CreatePdf(int acteId, Stream memoryStream);
        void CreatePdf(IEnumerable<int> acteIds, Stream memoryStream);
    }
}
