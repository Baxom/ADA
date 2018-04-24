using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ADA.Domain.Base;
using ADA.Data.Extention;
using ADA.Data.Context;
using ADA.Infrastructure.PaginationHandler;
using ADA.Data.Model;
using System.Data.SqlClient;
using System.Data.Common;

namespace ADA.Data.Repositories.Common
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        protected ADAContext Context { get; private set; }
        protected System.Data.Entity.DbSet<T> Set { get; private set; }
        
        public GenericRepository(ADAContext context)
        {
            Context = context;
            Set = context.Set<T>();

        }

        public virtual T FindBy(object key)
        {
            var intKey = key as Int32?;
            return intKey.HasValue ? Set.Where(b => b.Id == intKey.Value).FirstOrDefault() : null;
        }

        public virtual IList<T> Get(Expression<Func<T, bool>> filter = null,
                                   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                   params Expression<Func<T, object>>[] includeProperties)
        {
            return Query(filter, orderBy, includeProperties).ToList();
        }

        public virtual int Count(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = Set.AsQueryable();

            if (filter != null) query = query.Where(filter);

            return query.Count();
        }

        public virtual IList<T> FindAll()
        {
            return Set.ToList();
        }

        public virtual void Add(T item)
        {
            if (item == null) throw new ArgumentNullException(String.Format("entity : {0}", typeof(T).Name));

            Set.Add(item);
        }


        public virtual void Remove(T item)
        {
            if (item == null) throw new ArgumentNullException(String.Format("entity : {0}", typeof(T).Name));

            if (Context.Entry<T>(item).State == EntityState.Detached)
            {
                Set.Attach(item);
            }

            Set.Remove(item);
        }


        public PaginationResult<IList<T>> Paginate(PaginationRequest paginateRequest, Expression<Func<T, bool>> filter = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, object>>[] includeProperties)
        {
            var skip = (paginateRequest.PageNumber - 1) * paginateRequest.PageSize;
            var take = paginateRequest.PageSize;

            var count = this.Count(filter);

            var result = Query(filter, orderBy, includeProperties).Skip(skip).Take(take).ToList();

            return new PaginationResult<IList<T>>(count, result);
        }

        private IQueryable<T> Query(Expression<Func<T, bool>> filter = null,
                                   Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                   params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> query = Set.AsQueryable();
            
            
            if (filter != null) query = query.Where(filter);

            if (orderBy != null) query = orderBy(query);
            else query = query.OrderBy(b => b.Id);
  
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
                        
            return query;
        }

        public PaginationResult<IList<T>> PaginateWithTriModel(PaginationRequest paginateRequest, Expression<Func<T, bool>> filter = null, Model.TriModel orderBy = null, params Expression<Func<T, object>>[] includeProperties)
        {
            var skip = (paginateRequest.PageNumber - 1) * paginateRequest.PageSize;
            var take = paginateRequest.PageSize;

            var count = this.Count(filter);

            var result = Query(filter, orderBy, includeProperties).Skip(skip).Take(take).ToList();

            return new PaginationResult<IList<T>>(count, result);
        }

        public IList<T> GetWithTriModel(Expression<Func<T, bool>> filter = null, Model.TriModel orderBy = null, params Expression<Func<T, object>>[] includeProperties)
        {
            return Query(filter, orderBy, includeProperties).ToList();
        }

        private IQueryable<T> Query(Expression<Func<T, bool>> filter = null,
                                   TriModel orderBy = null,
                                   params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> query = Set.AsQueryable();

            if (filter != null) query = query.Where(filter);


          

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {

                var order = orderBy.GetOrder<T>();

                if (order != null)
                {
                    query = order(query);
                }
                else
                {
                    query = query.OrderBy(b => b.Id);
                }
            }

           

            return query;
        }

        protected int countQuery(string queryCount, params SqlParameter[] parameters)
        {

            int res = 0;
            DbDataReader reader = null;
            try
            {
                if (this.Context.Database.Connection.State == System.Data.ConnectionState.Closed) this.Context.Database.Connection.Open();

                var command = this.Context.Database.Connection.CreateCommand();

                command.CommandText = queryCount;
                command.Parameters.AddRange(parameters.Select( b => new SqlParameter(b.ParameterName, b.Value)).ToArray());

                reader = command.ExecuteReader();

                if (reader.Read())
                {
                    res = reader.GetInt32(0);
                }

            }
            finally
            {
                if (reader != null) reader.Close();
            }

            return res;
        }
    }
}
