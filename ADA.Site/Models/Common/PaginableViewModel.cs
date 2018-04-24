using ADA.Data.Model;
using ADA.Site.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADA.Site.Models.Common
{
    public abstract class PaginableViewModel : IPaginable
    {

        public int Page { get; set; }

        #region IPaginable

        public Paginable.PaginationViewModel Pagination { get; set; }

        #endregion

        public PaginableViewModel()
        {
            Page = 1;
            Pagination = new Paginable.PaginationViewModel();
        }


    }
}