using ADA.Data.SqlServer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ADA.Data.Intercepteurs
{
    static class LanguageExtensions
    {
        public static bool In<T>(this T source, params T[] list)
        {
            return (list as IList<T>).Contains(source);
        }
    }

    public class FtsInterceptor : IDbCommandInterceptor
    {
        static IFTSStringProvider _fTSStringProvider;

        public FtsInterceptor(IFTSStringProvider fTSStringProvider)
        {
            _fTSStringProvider = fTSStringProvider;
        }

        internal const string FullTextPrefix = "-FTSPREFIX-";
        public static string Fts(string search)
        {
            return string.Format("({0}{1})", FullTextPrefix, search);
        }
        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }
        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
        }
        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            RewriteFullTextQuery(command);
        }
        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
        }
        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            RewriteFullTextQuery(command);
        }
        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
        }
        public static void RewriteFullTextQuery(DbCommand cmd)
        {
            string text = cmd.CommandText;

            List<DbParameter> originalParameters = new List<DbParameter>(cmd.Parameters.Cast<DbParameter>().ToList());
            
            for (int i = 0; i < originalParameters.Count; i++)
            {
                DbParameter parameter = originalParameters[i];
                if (parameter.DbType.In(DbType.String, DbType.AnsiString, DbType.StringFixedLength, DbType.AnsiStringFixedLength))
                {
                    if (parameter.Value == DBNull.Value)
                        continue;
                    var value = (string)parameter.Value;
                    if (value.IndexOf(FullTextPrefix) >= 0)
                    {
                        parameter.Size = 4096;
                        parameter.DbType = DbType.AnsiStringFixedLength;
                        value = value.Replace(FullTextPrefix, ""); // remove prefix we added n linq query
                        value = value.Substring(2, value.Length - 4); // remove %% escaping by linq translator from string.Contains to sql LIKE

                        _fTSStringProvider.RewriteQuery(cmd, parameter, value);
                        //parameter.Value = _fTSStringProvider.BuildContainsPredicate(value);
                        //cmd.CommandText = _fTSStringProvider.RewriteQuery(text, parameter.ParameterName); 

                        //if (text == cmd.CommandText)
                        //    throw new Exception("FTS was not replaced on: " + text);
                        text = cmd.CommandText;
                    }
                }
            }
        }
    }
}