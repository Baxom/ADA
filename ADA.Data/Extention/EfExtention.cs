using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Reflection;
using ADA.Data.Model;

namespace ADA.Data.Extention
{
    public static class EfExtention
    {
        public static IQueryable<T> IncludeProperties<T>(this IQueryable<T> query, IEnumerable<Expression<Func<T, object>>> properties) where T : class
        {
            IQueryable<T> result = query;

            foreach (var property in properties)
            {
                result = result.Include(property);
            }

            return result;

        }
    }
}

//        static MethodInfo _deepOrderByMethod = typeof(EfExtention).GetMethods().Single(method =>
//                   method.Name == "DeepOrderByProperty" && method.GetParameters().Length == 2 && method.GetParameters().Last().ParameterType == typeof(string));

//        static MethodInfo _deepOrderByPropertyDescendingMethod = typeof(EfExtention).GetMethods().Single(method =>
//                   method.Name == "DeepOrderByPropertyDescending" && method.GetParameters().Length == 2);

//        static MethodInfo _enumerableOrderByMethod = typeof(Enumerable).GetMethods().Single(method =>
//               method.Name == "OrderBy" && method.GetParameters().Length == 2);

//        static MethodInfo _enumerableOrderByDescendingMethod = typeof(Enumerable).GetMethods().Single(method =>
//               method.Name == "OrderByDescending" && method.GetParameters().Length == 2);

//        static MethodInfo _queryableOrderByMethod = typeof(Queryable).GetMethods().Single(method =>
//                method.Name == "OrderBy" && method.GetParameters().Length == 2);

//        static MethodInfo _queryabletThenByMethod = typeof(Queryable).GetMethods().Single(method =>
//                method.Name == "ThenBy" && method.GetParameters().Length == 2);

//        static MethodInfo _orderByDescendingMethod = typeof(Queryable).GetMethods().Single(method =>
//               method.Name == "OrderByDescending" && method.GetParameters().Length == 2);

//        static MethodInfo _thenByDescendingMethod = typeof(Queryable).GetMethods().Single(method =>
//               method.Name == "ThenByDescending" && method.GetParameters().Length == 2);

//        static IList<MethodInfo> _minMethods = typeof(Enumerable).GetMethods().Where(method =>
//                method.Name == "Min" && method.GetParameters().Length == 2).ToList();


//        static IList<MethodInfo> _maxMethod = typeof(Enumerable).GetMethods().Where(method =>
//               method.Name == "Max" && method.GetParameters().Length == 2).ToList();

//        public static IEnumerable<T> OrderByProperty<T>(this IEnumerable<T> source, string property)
//        {

//            var parameterExpression = Expression.Parameter(typeof(T), "x");

//            var propertyName = property;
//            LambdaExpression lx = GetLambdaExpression(parameterExpression, ref propertyName, _minMethods);

//            MethodInfo genericOrderBy =
//               _enumerableOrderByMethod.MakeGenericMethod(typeof(T), lx.ReturnType);
//            object ret = genericOrderBy.Invoke(null, new object[] { source, lx.Compile() });

//            return (IEnumerable<T>)ret;
//        }

//        public static IEnumerable<T> OrderByPropertyDescending<T>(this IEnumerable<T> source, string property)
//        {

//            var parameterExpression = Expression.Parameter(typeof(T), "x");

//            var propertyName = property;
//            LambdaExpression lx = GetLambdaExpression(parameterExpression, ref propertyName, _minMethods);

//            MethodInfo genericOrderBy =
//               _enumerableOrderByDescendingMethod.MakeGenericMethod(typeof(T), lx.ReturnType);
//            object ret = genericOrderBy.Invoke(null, new object[] { source, lx.Compile() });

//            return (IEnumerable<T>)ret;
//        }

//        public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> source, string property)
//        {

//            var parameterExpression = Expression.Parameter(typeof(T), "x");

//            var propertyName = property;
//            LambdaExpression lx = GetLambdaExpression(parameterExpression, ref propertyName, _minMethods);

//            MethodInfo genericOrderBy =
//               _queryableOrderByMethod.MakeGenericMethod(typeof(T), lx.ReturnType);
//            object ret = genericOrderBy.Invoke(null, new object[] { source, lx });

//            return (IQueryable<T>)ret;
//        }

//        public static IQueryable<T> ThenByProperty<T>(this IQueryable<T> source, string property)
//        {

//            var parameterExpression = Expression.Parameter(typeof(T), "x");

//            var propertyName = property;
//            LambdaExpression lx = GetLambdaExpression(parameterExpression, ref propertyName, _minMethods);

//            MethodInfo genericThenBy =
//               _queryabletThenByMethod.MakeGenericMethod(typeof(T), lx.ReturnType);
//            object ret = genericThenBy.Invoke(null, new object[] { source, lx });

//            return (IQueryable<T>)ret;
//        }

//        public static IQueryable<T> OrderByPropertyDescending<T>(this IQueryable<T> source, string property)
//        {

//            var parameterExpression = Expression.Parameter(typeof(T), "x");

//            var propertyName = property;
//            LambdaExpression lx = GetLambdaExpression(parameterExpression, ref propertyName, _maxMethod);

//            MethodInfo genericOrderDescendingBy =
//               _orderByDescendingMethod.MakeGenericMethod(typeof(T), lx.ReturnType);
//            object ret = genericOrderDescendingBy.Invoke(null, new object[] { source, lx });

//            return (IQueryable<T>)ret;
//        }

//        public static IQueryable<T> ThenByPropertyDescending<T>(this IQueryable<T> source, string property)
//        {

//            var parameterExpression = Expression.Parameter(typeof(T), "x");

//            var propertyName = property;
//            LambdaExpression lx = GetLambdaExpression(parameterExpression, ref propertyName, _maxMethod);

//            MethodInfo genericThenDescendingBy =
//               _thenByDescendingMethod.MakeGenericMethod(typeof(T), lx.ReturnType);
//            object ret = genericThenDescendingBy.Invoke(null, new object[] { source, lx });

//            return (IQueryable<T>)ret;
//        }

//        private static LambdaExpression GetLambdaExpression(ParameterExpression parameterExpression, ref string propertyName, IList<MethodInfo> methods, int compteur = 0)
//        {
//            if (compteur > 1) throw new Exception("Impossible de de gerer + d'une expression lambda imbriquées.");

//            ParameterExpression newParameterExpression = null;
//            var memberExpression = GetMemberExpression(parameterExpression, ref propertyName);

//            if (String.IsNullOrEmpty(propertyName))
//            {
//                return Expression.Lambda(memberExpression, parameterExpression);
//            }
//            else
//            {
//                newParameterExpression = Expression.Parameter(memberExpression.Type.GenericTypeArguments.First(), parameterExpression.Name + compteur);
//                var lambda = GetLambdaExpression(newParameterExpression, ref propertyName, methods, ++compteur);

//                var method = methods.Single(b => b.ReturnType == lambda.ReturnType);

//                MethodInfo genericMin =
//                    method.MakeGenericMethod(memberExpression.Type.GenericTypeArguments.First());
//                return Expression.Lambda(Expression.Call(null, genericMin, memberExpression, lambda), parameterExpression);
//            }



//        }

//        private static Expression GetMemberExpression(Expression exp, ref string propertyName)
//        {
//            if (exp.Type.GetInterfaces().Select(b => b.Name).Contains("IEnumerable")) return exp;

//            if (propertyName.Contains("."))
//            {
//                int index = propertyName.IndexOf(".");
//                var subParam = Expression.Property(exp, propertyName.Substring(0, index));
//                propertyName = propertyName.Substring(index + 1);
//                return GetMemberExpression(subParam, ref propertyName);
//            }

//            var expressionFinal = Expression.Property(exp, propertyName);

//            propertyName = null;
//            return expressionFinal;
//        }

//        public static bool PropertyExists<T>(string propertyName)
//        {
//            return typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase |
//                BindingFlags.Public | BindingFlags.Instance) != null;
//        }

//        public static IEnumerable<T> DeepOrderByProperty<T>(this IEnumerable<T> source, string property)
//        {
//            var splitProperty = property.Split('.');
//            var compteur = 0;

//            foreach (var propertyPart in splitProperty)
//            {
//                compteur++;
//                var propertyType = typeof(T).GetProperty(propertyPart).PropertyType;

//                if (propertyType.GetInterfaces().Select(g => g.Name).Contains("IEnumerable") && propertyType.GenericTypeArguments.Any())
//                {
//                    source.ToList().ForEach(b =>
//                    {
//                        var subsplitProperty = String.Join(".", splitProperty.Skip(compteur).Take(splitProperty.Length - compteur));

//                        var subPropertyInstance = b.GetType().GetProperty(propertyPart).GetValue(b, null);

//                        MethodInfo genericDeepOrderBy =
//                                _deepOrderByMethod.MakeGenericMethod(propertyType.GenericTypeArguments.First());
//                        var propSubOrdered = genericDeepOrderBy.Invoke(null, new object[] { subPropertyInstance, subsplitProperty });

//                        var constructedListType = typeof(List<>).MakeGenericType(propertyType.GenericTypeArguments.First());
//                        var castedSubproperty = Activator.CreateInstance(constructedListType, new object[] { propSubOrdered });
//                        b.GetType().GetProperty(propertyPart).SetValue(b, castedSubproperty);
//                    });

//                    return source;
//                }
//            }

//            return source.OrderByProperty(property);
//        }

//        public static IEnumerable<T> DeepOrderByPropertyDescending<T>(this IEnumerable<T> source, string property)
//        {
//            var splitProperty = property.Split('.');
//            var compteur = 0;

//            foreach (var propertyPart in splitProperty)
//            {
//                compteur++;
//                var propertyType = typeof(T).GetProperty(propertyPart).PropertyType;

//                if (propertyType.GetInterfaces().Select(g => g.Name).Contains("IEnumerable") && propertyType.GenericTypeArguments.Any())
//                {
//                    source.ToList().ForEach(b =>
//                    {
//                        var subsplitProperty = String.Join(".", splitProperty.Skip(compteur).Take(splitProperty.Length - compteur));

//                        var subPropertyInstance = b.GetType().GetProperty(propertyPart).GetValue(b, null);

//                        MethodInfo genericDeepOrderBy =
//                                _deepOrderByPropertyDescendingMethod.MakeGenericMethod(propertyType.GenericTypeArguments.First());
//                        var propSubOrdered = genericDeepOrderBy.Invoke(null, new object[] { subPropertyInstance, subsplitProperty });

//                        var constructedListType = typeof(List<>).MakeGenericType(propertyType.GenericTypeArguments.First());
//                        var castedSubproperty = Activator.CreateInstance(constructedListType, new object[] { propSubOrdered });
//                        b.GetType().GetProperty(propertyPart).SetValue(b, castedSubproperty);
//                    });

//                    return source;
//                }
//            }

//            return source.OrderByPropertyDescending(property);
//        }

//        public static IQueryable<T> OrderByProperty<T>(
//                this IQueryable<T> source, TriModel tri)
//        {
//            IQueryable<T> temp = source;
//            int compteur = 0;

//            foreach (var triItem in tri.Tris)
//            {
//                if (compteur == 0)
//                {
//                    if (triItem.Value == SortDirection.Ascendant)
//                    {
//                        temp = (IQueryable<T>)temp.OrderByProperty(triItem.Key);
//                    }
//                    else temp = (IQueryable<T>)temp.OrderByPropertyDescending(triItem.Key);
//                }
//                else
//                {
//                    if (triItem.Value == SortDirection.Ascendant)
//                    {
//                        temp = (IQueryable<T>)temp.ThenByProperty(triItem.Key);
//                    }
//                    else temp = (IQueryable<T>)temp.ThenByPropertyDescending(triItem.Key);
//                }
//                ++compteur;
//            }

//            return temp;

//        }

//        public static IEnumerable<T> DeepOrderByProperty<T>(
//               this IEnumerable<T> source, TriModel tri)
//        {
//            IEnumerable<T> temp = source;
//            int compteur = 0;

//            var triItem = tri.Tris.First();
//            {

//                if (triItem.Value == SortDirection.Ascendant)
//                {
//                    temp = (IEnumerable<T>)temp.DeepOrderByProperty(triItem.Key);
//                }
//                else temp = (IEnumerable<T>)temp.DeepOrderByPropertyDescending(triItem.Key);

//                return temp;

//            }
//        }

//    }
//}
