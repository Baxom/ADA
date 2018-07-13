using ADA.Data.Context;
using ADA.Data.Model;
using ADA.Data.Repositories.Common;
using ADA.Data.Repositories.Interfaces;
using ADA.Domain.Constantes;
using ADA.Domain.Fonds;
using ADA.Domain.Pretres;
using ADA.Infrastructure.PaginationHandler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Data.Repositories.Core
{
    public class FondMediumRepository : GenericRepository<FondMedium>, IFondMediumRepository
    {
        public FondMediumRepository(ADAContext context)
            : base(context)
        {

        }

        public PaginationResult<IList<int>> GetFondMediumIds(int fondId, string searchTerms, IEnumerable<int> index, List<Tuple<int, string, TypeColonneFond>> informationsFilters,
            PaginationRequest paginationRequest)
        {
            List<int> result = new List<int>();
            DbDataReader reader = null;
            int nbResult = 0;
            try
            {
                if (Context.Database.Connection.State != ConnectionState.Open) Context.Database.Connection.Open();

                var cmd = Context.Database.Connection.CreateCommand();

                // CREATION PARAMETRES
                DataTable dtIndex = new DataTable();
                dtIndex.Columns.Add("Id", typeof(int));
                index.ToList().ForEach(b => dtIndex.Rows.Add(b));

                DataTable dtFondInformationValue = new DataTable();
                dtFondInformationValue.Columns.Add("Id", typeof(int));
                dtFondInformationValue.Columns.Add("Value", typeof(string));
                informationsFilters.ToList().ForEach(b => dtFondInformationValue.Rows.Add(b.Item1, GetValue(b.Item2, b.Item3)));

                SqlParameter searchTermsParams = new SqlParameter("@searchTerm", SqlDbType.NVarChar) { Value = searchTerms };
                SqlParameter fondIdParams = new SqlParameter("@fondId", SqlDbType.Int) { Value = fondId };
                SqlParameter indexPraram = new SqlParameter("@index", SqlDbType.Structured) { Value = dtIndex };
                SqlParameter fondInformationValueParam = new SqlParameter("@FondInformationValues", SqlDbType.Structured) { Value = dtFondInformationValue };
                SqlParameter pageNumberParams = new SqlParameter("@pageNumber", SqlDbType.Int) { Value = paginationRequest.PageNumber };
                SqlParameter pageSizeParams = new SqlParameter("@pageSize", SqlDbType.Int) { Value = paginationRequest.PageSize };


                cmd.CommandText = "dbo.GetMediaFond";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(searchTermsParams);
                cmd.Parameters.Add(fondIdParams);
                cmd.Parameters.Add(indexPraram);
                cmd.Parameters.Add(fondInformationValueParam);
                cmd.Parameters.Add(pageNumberParams);
                cmd.Parameters.Add(pageSizeParams);

                reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    result.Add(reader.GetInt32(0));
                }

                reader.NextResult();
                reader.Read();
                nbResult = reader.GetInt32(0);

            }
            finally
            {
                if (reader != null) reader.Close();
                if (Context.Database.Connection.State == ConnectionState.Open) Context.Database.Connection.Close();
            }

            return new PaginationResult<IList<int>>(nbResult, result);
        }

        private string GetValue(string val, TypeColonneFond type)
        {
            if (type == TypeColonneFond.Date)
            {
                DateTime date = DateTime.Today;

                if (DateTime.TryParse(val, out date))
                    return date.ToString("dd/MM/yyyy");
                else return null;
            }
            else return String.IsNullOrEmpty(val) ? null : val;
        }
    }
}
