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
using ADA.Site.Models.Index;
using ADA.Domain.Fonds;
using ADA.Domain.Constantes;

namespace ADA.Site.Models
{
    public class RechercheFondViewModel : PaginableViewModel, ISearchable
    {
        public int? Id { get; set; }
        public IEnumerable<int> Index { get; set; }
        public List<Tuple<int, string, TypeColonneFond>> InformationsFilters { get; set; }

        public Fond Fond { get; set; }
        public IEnumerable<GroupeIndexViewModel> GroupeIndex { get; set; }
        public IDictionary<int, int?> SelectedIndex { get; set; }

        [DisplayName("Nom")]
        public string Nom { get; set; }
              
        public bool Recherche { get; set; }
        
        [System.Runtime.Serialization.IgnoreDataMember]
        public IPagedList<FondMedium> Resultats { get; set; }

        public RechercheFondViewModel()
            : this(true)
        {

        }

        public RechercheFondViewModel(bool activeSearch)
        {
            SelectedIndex = new Dictionary<int, int?>();
            Recherche = activeSearch;
            InformationsFilters = new List<Tuple<int, string, TypeColonneFond>>();
        }


       
    }
}
