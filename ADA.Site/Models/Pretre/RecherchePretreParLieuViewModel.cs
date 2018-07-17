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
    public class RecherchePretreParLieuViewModel : PaginableTriableViewModel, ISearchable
    {
        public int? TypeLieuId { get; set; }
        public int? LieuId { get; set; }
        public string NomLieu { get; set; }

        [DisplayName("Fonction")]
        public int? FonctionId { get; set; }

        [DisplayName("Contexte historique")]
        public int? ContextHistoriqueId { get; set; }

        [DisplayName("Année exercice")]
        public int? AnneeExercice { get; set; }

        public FonctionLieuViewModel FonctionLieuViewModel { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        public IPagedList<Pretre> Resultats { get; set; }
        
        public bool Recherche { get; set; }

        public RecherchePretreParLieuViewModel() : this(true)
        {

        }

        public RecherchePretreParLieuViewModel(bool activeSearch) : base()
        {
            Recherche = activeSearch;
            FonctionLieuViewModel = new FonctionLieuViewModel();
            this.TypeLieuId = null;

            InitTris();
        }

        protected override void InitTris()
        {
            AddTri<Pretre>("Nom prénom croissant", o => o.OrderBy(p => p.Nom).ThenBy(p => p.Prenom));
            AddTri<Pretre>("Nom prénom décroissant", o => o.OrderByDescending( p => p.Nom).ThenByDescending( p => p.Prenom ));
            AddTri<Pretre>("Année début croissante", o => o.OrderBy( p => p.FonctionsLieu.Min( fl => fl.AnneeDebut) ));
            AddTri<Pretre>("Année début décroissante", o => o.OrderByDescending( p => p.FonctionsLieu.Max( fl => fl.AnneeDebut) ));
            AddTri<Pretre>("Année fin croissante", o => o.OrderBy( p => p.FonctionsLieu.Min( fl => fl.AnneeFin) ));
            AddTri<Pretre>("Année fin décroissante", o => o.OrderByDescending( p => p.FonctionsLieu.Max( fl => fl.AnneeFin) ));

            Tri = Tris.First();
        }



    }
}
