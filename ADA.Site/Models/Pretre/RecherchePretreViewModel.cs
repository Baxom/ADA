using ADA.Data.Model;
using ADA.Domain.Pretres;
using ADA.Site.Models.Common;
using ADA.Site.Models.Interface;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ADA.Site.Models
{
    public class RecherchePretreViewModel : PaginableTriableViewModel, ISearchable
    {
        [DisplayName("Nom")]
        public string Nom { get; set; }
        [DisplayName("Prénom")]
        public string Prenom { get; set; }
        [DisplayName("Année naissance")]
        public int? AnneeNaissance { get; set; }
        [DisplayName("Année décès")]
        public int? AnneeDeces { get; set; }
        [DisplayName("Commune naissance")]
        public string Commune { get; set; }

        public bool Recherche { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        public IPagedList<Pretre> Resultats { get; set; }


        public RecherchePretreViewModel() : this(true)
        {
        }

        public RecherchePretreViewModel(bool activeSearch)
        {
            Recherche = activeSearch;
            InitTris();
        }

        protected override void InitTris()
        {
            AddTri<Pretre>("Nom prénom croissant", o => o.OrderBy( p => p.Nom).ThenBy( p => p.Prenom ));
            AddTri<Pretre>("Nom prénom décroissant", o => o.OrderByDescending( p => p.Nom).ThenByDescending( p => p.Prenom ));
            AddTri<Pretre>("Naissance croissante", o => o.OrderBy( p => p.AnneeNaissance));
            AddTri<Pretre>("Naissance décroissante", o => o.OrderByDescending( p => p.AnneeNaissance));
            AddTri<Pretre>("Décès croissant", o => o.OrderBy( p => p.AnneeDeces));
            AddTri<Pretre>("Décès décroissant", o => o.OrderByDescending( p => p.AnneeDeces));
         
            Tri = Tris.First();
        }
        
    }
}