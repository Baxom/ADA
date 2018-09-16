using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Infrastructure.Extentions
{
    public static class IListExtention
    {
        public static List<T> OrderByExtList<T,U>(this IList<T> source, List<U> orderer, Func<T, object> sourceKey = null, Func<U, object> ordererKey = null)
        {
            if (sourceKey == null) sourceKey = b => b;
            if (ordererKey == null) ordererKey = b => b;

            return orderer.Join(source, src => ordererKey(src), inner => sourceKey(inner), (src, inner) => inner).ToList();
        }

    }
}
