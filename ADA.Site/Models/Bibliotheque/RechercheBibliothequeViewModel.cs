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
using ADA.Domain.Bibliotheques;

namespace ADA.Site.Models
{
    public class RechercheBibliothequeViewModel : PaginableViewModel, ISearchable
    {   
        
        [DisplayName("Nom")]
        public string Nom { get; set; }

        [DisplayName("Auteur")]
        public string Auteur { get; set; }

        public bool Recherche { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        public IPagedList<Bibliotheque> Resultats { get; set; }

        public RechercheBibliothequeViewModel() : this(true)
        {
            
        }

        public RechercheBibliothequeViewModel(bool activeSearch)
        {
            this.Recherche = activeSearch;
        }
     
    }
}