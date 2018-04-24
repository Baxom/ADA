using ADA.Data.Context;
using ADA.Data.Model;
using ADA.Data.Repositories.Common;
using ADA.Data.Repositories.Interfaces;
using ADA.Domain.Pretres;
using ADA.Infrastructure.PaginationHandler;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Data.Repositories.Core
{
    public class PretreRepository : GenericRepository<Pretre>, IPretreRepository
    {
        public PretreRepository(ADAContext context)
            : base(context)
        {

        }

        //        public IList<Pretre> GetPretreByLieu(int? typeLieuId, int? lieuId, int? fonctionId, string nomLieu, TriModel tri, PaginationRequest paginationRequest)
        //        {
        //            var setPretreFonctionsLieu = this.Context.Set<PretreFonctionLieu>();
        //            var query = (this
        //                .Set
        //                .Join(setPretreFonctionsLieu, p => p.Id, pfl => pfl.PretreId, (pretres, pfls) => new { pretres = pretres, pfls = pfls })
        //                .OrderBy(b => b.pfls.AnneeDebut)
        //                .Select(b => b.pretres)) as System.Data.Entity.Infrastructure.DbQuery<Pretre>;

        //            query = query.Include("FonctionsLieu");
        //          /*  var query = this
        //               .Set.Include("FonctionsLieu")
        //               .Where( b => b.FonctionsLieu.Any())
        //               .OrderBy(b => b.FonctionsLieu.Min( fl => fl.AnneeDebut));*/


        //            return query.ToList();
        //        }

        //        public PaginationResult<IList<Pretre>> GetPretre(string nom, string prenom, int? anneeNaissance, int? anneeDeces, 
        //            string commune, TriModel tri, PaginationRequest paginationRequest)
        //        {
        //            var queryCount = buildQuery(nom, prenom, anneeNaissance, anneeDeces, commune, tri, paginationRequest, true);
        //            var queryPretre = buildQuery(nom, prenom, anneeNaissance, anneeDeces, commune, tri, paginationRequest);

        //            List<SqlParameter> parameters = new List<SqlParameter>();

        //            parameters.Add(new SqlParameter("@nom", nom));
        //            parameters.Add(new SqlParameter("@prenom", prenom));
        //            parameters.Add(new SqlParameter("@anneeNaissance", anneeNaissance));
        //            parameters.Add(new SqlParameter("@anneeDeces", anneeDeces));
        //            parameters.Add(new SqlParameter("@commune", commune));

        //            parameters.ToList().ForEach(p => { if (p.Value == null) { p.Value = (object)DBNull.Value; } });

        //            var pretres = this.Context.Set<Pretre>().SqlQuery(queryPretre, parameters.ToArray()).ToList();
        //            int countPretre = countQuery(queryCount, parameters.ToArray());

        //            return new PaginationResult<IList<Pretre>>(countPretre, pretres);
        //        }


        //        private string buildQuery(string nom, string prenom, int? anneeNaissance, int? anneeDeces,
        //            string commune, TriModel tri, PaginationRequest paginationRequest, bool count = false)
        //        {
        //            string select = String.Format("SELECT {0} FROM Pretre P LEFT JOIN Commune C ON C.Id = P.CommuneId ", count ? "COUNT(1)" : "P.*");
        //            string where = @"WHERE 
        //                            (@nom IS NULL OR @nom = '' OR p.nom like '%' + @nom + '%' ) 
        //                            AND (@prenom IS NULL OR @prenom = '' OR p.prenom like '%' + @prenom + '%' )
        //                            AND (@commune IS NULL OR @commune = '' OR c.nom like '%' + @commune + '%' )
        //                            AND (@anneeNaissance IS NULL OR p.AnneeNaissance = @anneeNaissance)
        //                            AND (@anneeDeces IS NULL OR p.AnneeDeces = @anneeNaissance) ";

        //            string orderBy = string.Empty;
        //            string pagination = string.Empty;


        //            if (!count)
        //            {
        //                orderBy = tri.GetOrderBy();
        //                if (String.IsNullOrEmpty(orderBy)) orderBy = " ORDER BY P.Id ";
        //                pagination = String.Format(@" OFFSET {0} ROWS -- skip 10 rows
        //                                                FETCH NEXT {1} ROWS ONLY; ", (paginationRequest.PageNumber - 1) * paginationRequest.PageSize, paginationRequest.PageSize);
        //            }

        //            return select + where + orderBy + pagination;
        //        }
        //    }
    }
}
