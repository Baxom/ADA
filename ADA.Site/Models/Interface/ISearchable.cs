using ADA.Data.Model;
using ADA.Site.Models.Paginable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Site.Models.Interface
{
    public interface ISearchable
    {
        bool Recherche { get; set; }
    }
}
