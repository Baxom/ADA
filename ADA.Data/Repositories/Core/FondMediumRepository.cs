using ADA.Data.Context;
using ADA.Data.Helpers;
using ADA.Data.Model;
using ADA.Data.Repositories.Common;
using ADA.Data.Repositories.Interfaces;
using ADA.Data.SqlServer.Interface;
using ADA.Domain.Constantes;
using ADA.Domain.Fonds;
using ADA.Domain.Pretres;
using ADA.Infrastructure.PaginationHandler;
using ADA.Infrastructure.Services.Interface.WordSearchParser;
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
        IWordSearchParser _wordSearchParser;

        public FondMediumRepository(ADAContext context, IWordSearchParser wordSearchParser)
            : base(context)
        {
            _wordSearchParser = wordSearchParser;
        }

        public PaginationResult<IList<FondMedium>> GetFondMedium(int fondId, string searchTerms, IEnumerable<int> index, List<Tuple<int, string, TypeColonneFond>> informationsFilters,
            PaginationRequest paginationRequest)
        {

            ResultAndCountModel result;

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

                DataTable dtWordSearch = new DataTable();
                dtWordSearch.Columns.Add("Libelle", typeof(string));
                dtWordSearch.Columns.Add("WordBoundary", typeof(bool));
                _wordSearchParser.Parse(searchTerms).ToList().ForEach(b => dtWordSearch.Rows.Add(b.Val, b.WordBoundary));

                SqlParameter searchTermsParams = new SqlParameter("@searchTerm", SqlDbType.Structured) { Value = dtWordSearch };
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

                result = SqlDataReaderHelper.ExecuteCommand(cmd);

            }
            finally
            {
                if (Context.Database.Connection.State == ConnectionState.Open) Context.Database.Connection.Close();
            }


            var fondMediums = this.Get(b => result.Ids.Contains(b.Id),
               b => b.OrderByDescending(o => o.Titre.Contains(searchTerms)), b => b.ColonneFondMedium, 
               b => b.Fond.InformationAffichageFonds, 
               b => b.ColonneFondMedium.Select( col => col.InformationFond),
               b => b.Tags.Select(t => t.Tag));

            return new PaginationResult<IList<FondMedium>>(result.Count, fondMediums);
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
