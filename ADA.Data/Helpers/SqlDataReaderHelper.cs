using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Data.Helpers
{
    public static class SqlDataReaderHelper
    {
        public static ResultAndCountModel ExecuteCommand(DbCommand dbCommand)
        {
            List<int> ids = new List<int>();
            DbDataReader reader = null;
            int nbResult = 0;

            try
            {
                reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    ids.Add(reader.GetInt32(0));
                }

                reader.NextResult();
                reader.Read();
                nbResult = reader.GetInt32(0);
            }
            finally
            {

                if (reader != null) reader.Close();
            }

            return new ResultAndCountModel(ids, nbResult);
        }
    }
}
