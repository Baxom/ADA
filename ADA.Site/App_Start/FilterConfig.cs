using ADA.Site.Filter;
using System.Web;
using System.Web.Mvc;

namespace ADA.Site
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ErrorFilter());
        }
    }
}
