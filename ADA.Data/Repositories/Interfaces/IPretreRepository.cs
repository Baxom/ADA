using ADA.Data.Model;
using ADA.Data.Repositories.Common;
using ADA.Domain.Pretres;
using ADA.Infrastructure.PaginationHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Data.Repositories.Interfaces
{
    public interface IPretreRepository : IGenericRepository<Pretre>
    {
        //PaginationResult<IList<Pretre>> GetPretre(string nom, string prenom, int? anneeNaissance, int? anneeDeces, string commune, TriModel tri, PaginationRequest paginationRequest);
       // IList<Pretre> GetPretreByLieu(int? typeLieuId, int? lieuId, int? fonctionId, string nomLieu, TriModel tri, PaginationRequest paginationRequest);
    }
}
