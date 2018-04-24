using ADA.Data.Model;
using ADA.Domain.Base;
using ADA.Site.Models.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace ADA.Site.Models.Common
{
    public abstract class PaginableTriableViewModel : ITriable, IPaginable
    {

        public int Page { get; set; }

        #region ITriable

        public TriModel Tri { get; set; }
        public List<TriModel> Tris { get; private set; }

        #endregion

        #region IPaginable

        public Paginable.PaginationViewModel Pagination { get; set; }

        #endregion

        public PaginableTriableViewModel()
        {
            Page = 1;
            Pagination = new Paginable.PaginationViewModel();
            Tris = new List<TriModel>();

        }

        protected void AddTri<T>(string libelle, Func<IQueryable<T>, IOrderedQueryable<T>> order) where T : IEntity
        {
            var indice = Tris.Count();

            Tris.Add(new TriModel<T>(indice, libelle, order));
        }

        protected abstract void InitTris();

    }
}