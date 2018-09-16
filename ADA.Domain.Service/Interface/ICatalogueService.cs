using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Services.Interface
{
    public interface ICatalogueService
    {
        void CreatePdf(int catalogueId, string searchPhrase, Stream memoryStream);
        void CreatePdf(IEnumerable<int> catalogueIds, string searchPhrase, Stream memoryStream);
    }
}
