using ADA.Data.SqlServer.Interface;
using ADA.Infrastructure.Services.Interface.WordSearchParser;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ADA.Data.SqlServer.Core
{
    public class SqlServerStringProvider : IFTSStringProvider
    {
        IWordSearchParser _wordSearchParser;

        public SqlServerStringProvider(IWordSearchParser wordSearchParser)
        {
            _wordSearchParser = wordSearchParser;
        }

        public string BuildContainsPredicate(string inputSearch)
        {
            var predicate = String.Join(" AND ", _wordSearchParser.Parse(inputSearch).Select(b => BuildPredicate(b)));

            return String.IsNullOrWhiteSpace(predicate) ? null : String.Format("({0})", predicate);
        }


        void IFTSStringProvider.RewriteQuery(DbCommand command, DbParameter param, string inputSearch)
        {
            List<DbParameter> parameters = new List<DbParameter>();
            var parameterNameBase = param.ParameterName;
            var paramCount = 0;
            string text = command.CommandText;
            var words = _wordSearchParser.Parse(inputSearch)
                .Select(b => new { ParameterName = parameterNameBase + "_custom_" + (paramCount++).ToString(), WordSearch = b })
                .ToList();
            param.Value = inputSearch;
            if (words.Any())
            {
                var likeFilter = String.Join(" AND ", words.Select(b => {
                    if (b.WordSearch.WordBoundary)
                    {
                        return " '#' + [$1].[$2] + '#' LIKE @" + b.ParameterName + "  ESCAPE N'~' ";
                    }
                    return " [$1].[$2]  LIKE @" + b.ParameterName + "  ESCAPE N'~' "; ;

                }));

                command.CommandText = Regex.Replace(command.CommandText,
                        string.Format(
                        @"\[(\w*)\].\[(\w*)\]\s*LIKE\s*@{0}\s?(?:ESCAPE N?'~')", parameterNameBase),
                        likeFilter);

                if(text != command.CommandText)
                    command.Parameters.AddRange(words.Select(b => new SqlParameter()
                    {
                        ParameterName = b.ParameterName,
                        Value = b.WordSearch.WordBoundary ? "%[^a-z]" + b.WordSearch.Val + "[^a-z]%" : "%" + b.WordSearch.Val + "%",
                        DbType = System.Data.DbType.String
                    }).ToArray());
            }
        }

      

        public string RewriteQuery(string query, string parameterName)
        {
            return Regex.Replace(query,
                        string.Format(
                        @"\[(\w*)\].\[(\w*)\]\s*LIKE\s*@{0}\s?(?:ESCAPE N?'~')", parameterName),
                        string.Format(@"contains([$1].[$2], @{0})", parameterName));
        }

        private string BuildPredicate(Word word)
        {
            if(word is CompleteWord)
            {
                return String.Format("\"{0}\"", word.Val);
            }
            else return String.Format("\"{0}*\"", word.Val);
        }
        
        public Expression<Func<T, bool>> BuildFilterExpression<T>(Expression<Func<T, bool>> exprBase, string searchTerms, Expression<Func<T, string>> property)
        {
            ParameterExpression paramExprBase = exprBase.Parameters.First();
            MethodInfo methodContains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            MethodInfo methodConcat = typeof(string).GetMethod("Concat", new[] { typeof(string) });

            string PropertyName = ((MemberExpression)property.Body).Member.Name;

            Expression propertyParam = Expression.Property(paramExprBase, PropertyName);


            propertyParam = Expression.Add(Expression.Constant("#"), propertyParam, methodConcat);
            propertyParam = Expression.Add(propertyParam, Expression.Constant("#"), methodConcat);

            Expression filter = Expression.Call(propertyParam, methodContains, Expression.Constant(searchTerms));

            

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(filter, exprBase.Body), exprBase.Parameters);
        }


        //public Expression<Func<T, bool>> BuildFilterExpression<T>(Expression<Func<T, bool>> exprBase, string searchTerms, Expression<Func<T, string>> property)
        //{
        //    ParameterExpression paramExprBase = exprBase.Parameters.First();

        //    MethodInfo method = typeof(string).GetMethod("Contains", new[] { typeof(string) });

        //    string PropertyName = ((MemberExpression)property.Body).Member.Name;

        //    Expression propertyParam = Expression.Property(paramExprBase, PropertyName);
        //    Expression filter = Expression.Call(propertyParam, method, Expression.Constant(searchTerms));



        //    return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(filter, exprBase.Body), exprBase.Parameters);
        //}

    }
}
