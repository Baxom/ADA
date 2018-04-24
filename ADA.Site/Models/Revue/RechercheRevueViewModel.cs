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

namespace ADA.Site.Models
{
    public class RechercheRevueViewModel : PaginableViewModel, ISearchable
    {
        public bool RechercheGlobale { get { return String.IsNullOrWhiteSpace(CodeRevue); } }

        public string CodeRevue { get; set; }
        public IEnumerable<int> Index { get; set; }

        public Revue Revue { get; set; }
        public bool ChampsRechercheParAnneeVisible { get { return Revue != null && Revue.RechercheParAnnee; } }
        public bool ChampsRechercheParParoissesVisible { get { return Revue != null && Revue.RechercheParParoisse; } }
        public IEnumerable<GroupeIndexViewModel> GroupeIndex { get; set; }
        public IDictionary<int, int?> SelectedIndex { get; set; }

        [DisplayName("Nom")]
        public string Nom { get; set; }

        [DisplayName("Annee de début")]
        public Int32? AnneeDebut { get; set; }

        [DisplayName("Annee de Fin")]
        public Int32? AnneeFin { get; set; }

        [DisplayName("Auteur")]
        public string Auteur { get; set; }

        [DisplayName("Paroisse")]
        public string Paroisse { get; set; }

        public bool Recherche { get; set; }

        public List<Revue> RevuesDisponibles { get; set; }
        public List<int> RevuesSelectionnees { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        public IPagedList<ArticleRevue> Resultats { get; set; }

        public RechercheRevueViewModel()
            : this(true)
        {

        }

        public RechercheRevueViewModel(bool activeSearch)
        {
            SelectedIndex = new Dictionary<int, int?>();
            RevuesSelectionnees = new List<int>();
            RevuesDisponibles = new List<Revue>();
            Recherche = activeSearch;
        }


       
    }
}