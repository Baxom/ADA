using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Data.SqlServer.Interface
{
    public interface IFTSStringProvider
    {
        string BuildContainsPredicate(string inputSearch);
        string RewriteQuery(string query, string parameterName);

        void RewriteQuery(DbCommand command, DbParameter param, string inputSearch);


    }
}
