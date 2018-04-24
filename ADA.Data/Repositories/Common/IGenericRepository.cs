using ADA.Data.Model;
using ADA.Domain.Base;
using ADA.Infrastructure.PaginationHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Data.Repositories.Common
{
    public interface IGenericRepository<T> where T : class, IEntity
    {
        T FindBy(object key);
        PaginationResult<IList<T>> Paginate(PaginationRequest paginateRequest, Expression<Func<T, bool>> filter = null,
                                   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                   params Expression<Func<T, object>>[] includeProperties);
        PaginationResult<IList<T>> PaginateWithTriModel(PaginationRequest paginateRequest, Expression<Func<T, bool>> filter = null,
                                   TriModel orderBy = null,
                                   params Expression<Func<T, object>>[] includeProperties);
        IList<T> Get(Expression<Func<T, bool>> filter = null,
                                   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                   params Expression<Func<T, object>>[] includeProperties);
        IList<T> GetWithTriModel(Expression<Func<T, bool>> filter = null,
                                   TriModel orderBy = null,
                                   params Expression<Func<T, object>>[] includeProperties);
        int Count(Expression<Func<T, bool>> filter = null);
        IList<T> FindAll();
        void Add(T item);
        void Remove(T item);



    }
}
