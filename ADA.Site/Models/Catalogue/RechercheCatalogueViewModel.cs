using ADA.Data.Model;
using ADA.Domain.Pretres;
using ADA.Domain.Revues;
using ADA.Site.Models.Common;
using ADA.Site.Models.Interface;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using ADA.Domain.Catalogues;

namespace ADA.Site.Models
{
    public class RechercheCatalogueViewModel : PaginableViewModel, ISearchable
    {   
        [DisplayName("Titre")]
        public string Titre { get; set; }

        public int? SerieId { get; set; }
        public int? SousSerieId { get; set; }

        [DisplayName("Sous-series")]
        public SousSerie SousSerie { get; set; }

        public bool Recherche { get; set; }

        [DisplayName("Series")]
        public IEnumerable<Serie> Series { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        public IPagedList<Catalogue> Resultats { get; set; }

        public RechercheCatalogueViewModel() : this(true)
        {
            
        }

        public RechercheCatalogueViewModel(bool activeSearch)
        {
            this.Recherche = activeSearch;
        }
     
    }
}
