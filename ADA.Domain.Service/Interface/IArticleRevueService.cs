using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Services.Interface
{
    public interface IArticleRevueService
    {
        void CreatePdf(int articleRevueId, Stream memoryStream);
        void CreatePdf(IEnumerable<int> articleRevueIds, Stream memoryStream);
    }
}
