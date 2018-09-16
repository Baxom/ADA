using ADA.Data.Context;
using ADA.Data.Helpers;
using ADA.Data.Model;
using ADA.Data.Repositories.Common;
using ADA.Data.Repositories.Interfaces;
using ADA.Domain.Pretres;
using ADA.Infrastructure.PaginationHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using ADA.Infrastructure.Extentions;
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

        public PaginationResult<IList<Pretre>> GetPretreByLieu(int? typeLieuId, int? lieuId, int? fonctionId, string nomLieu, 
            int? contextHistoriqueId, int? anneeExercice, TriModel tri, PaginationRequest paginationRequest)
        {

            ResultAndCountModel result;

            try
            {
                if (Context.Database.Connection.State != ConnectionState.Open) Context.Database.Connection.Open();

                var cmd = Context.Database.Connection.CreateCommand();
                
                SqlParameter stypeLieuIdParams = new SqlParameter("@typeLieuId", SqlDbType.Int) { Value = typeLieuId };
                SqlParameter lieuIdParams = new SqlParameter("@lieuId", SqlDbType.Int) { Value = lieuId };
                SqlParameter fonctionIdPraram = new SqlParameter("@fonctionId", SqlDbType.Int) { Value = fonctionId };
                SqlParameter nomLieuParam = new SqlParameter("@nomLieu", SqlDbType.NVarChar) { Value = nomLieu };
                SqlParameter contextHistoriqueParam = new SqlParameter("@contextHistoriqueId", SqlDbType.Int) { Value = contextHistoriqueId };
                SqlParameter anneeExerciceParam = new SqlParameter("@anneeExercice", SqlDbType.Int) { Value = anneeExercice };
                SqlParameter pageNumberParams = new SqlParameter("@pageNumber", SqlDbType.Int) { Value = paginationRequest.PageNumber };
                SqlParameter pageSizeParams = new SqlParameter("@pageSize", SqlDbType.Int) { Value = paginationRequest.PageSize };
                SqlParameter triParams = new SqlParameter("@tri", SqlDbType.Int) { Value = tri.Key };


                cmd.CommandText = "dbo.GetPretreFonctionLieu";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(stypeLieuIdParams);
                cmd.Parameters.Add(lieuIdParams);
                cmd.Parameters.Add(fonctionIdPraram);
                cmd.Parameters.Add(contextHistoriqueParam);
                cmd.Parameters.Add(nomLieuParam);
                cmd.Parameters.Add(anneeExerciceParam);
                cmd.Parameters.Add(triParams);
                cmd.Parameters.Add(pageNumberParams);
                cmd.Parameters.Add(pageSizeParams);

                result = SqlDataReaderHelper.ExecuteCommand(cmd);

            }
            finally
            {
                if (Context.Database.Connection.State == ConnectionState.Open) Context.Database.Connection.Close();
            }


            var pretres = this.Get(b => result.Ids.Contains(b.Id), null, b => b.Photos,
                            b => b.Documents,
                            b => b.ArticlesRevue,
                            b => b.FonctionsLieu.Select(fl => fl.Fonction),
                            b => b.FonctionsLieu.Select(fl => fl.Lieu)).OrderByExtList<Pretre, int>(result.Ids.ToList(), b => b.Id);

            return new PaginationResult<IList<Pretre>>(result.Count, pretres);

            
        }

    }
}
