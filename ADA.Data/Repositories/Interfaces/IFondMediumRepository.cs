using ADA.Data.Model;
using ADA.Data.Repositories.Common;
using ADA.Domain.Constantes;
using ADA.Domain.Fonds;
using ADA.Domain.Pretres;
using ADA.Infrastructure.PaginationHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Data.Repositories.Interfaces
{
    public interface IFondMediumRepository : IGenericRepository<FondMedium>
    {
        PaginationResult<IList<FondMedium>> GetFondMedium(int fondId, string searchTerms, IEnumerable<int> index, List<Tuple<int, string, TypeColonneFond>> informationsFilters, PaginationRequest paginationRequest);
       
    }
}
