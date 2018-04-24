using ADA.Infrastructure.PaginationHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace ADA.Site.Extentions
{
    public static class PaginationResultExtention
    {
        /// <summary>
        /// Transforme un PaginnationResult au format accepté par PagedList.Mvc 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IPagedList<T> ToPagedListMvc<T>(this PaginationResult<IList<T>>  list, int pagedNnumber, int pageSize)
        {
            return new StaticPagedList<T>(list.Data, pagedNnumber, pageSize, list.CountResult);
        }
    }
}