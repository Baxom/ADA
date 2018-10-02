using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADA.Domain.Constantes;
using System.Linq.Expressions;
using System.Reflection;
using ADA.Domain.Base;
using System.Text;

namespace ADA.Data.Model
{
    public abstract class TriModel
    {
        public int Key { get; private set; }
        public string Libelle { get; private set; }

        public TriModel(int key, string libelle)
        {
            Key = key;
            Libelle = libelle;
        }

        public abstract Func<IQueryable<T>, IOrderedQueryable<T>> GetOrder<T>() where T : IEntity;
    }

    public class TriModel<T> : TriModel where T : IEntity
    {
        public Func<IQueryable<T>, IOrderedQueryable<T>> Order { get; private set; }


        public TriModel(int key, string libelle, Func<IQueryable<T>, IOrderedQueryable<T>> order)
            : base(key, libelle)
        {
            Order = order;
        }

        public override Func<IQueryable<U>, IOrderedQueryable<U>> GetOrder<U>()
        {
            return (Func<IQueryable<U>, IOrderedQueryable<U>>)Order;
        }
       
    }
}
