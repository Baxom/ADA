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
using ADA.Domain.Media;
using ADA.Domain.Constantes;

namespace ADA.Site.Models
{
    public class RechercheMediaViewModel : PaginableViewModel, ISearchable
    {   
        
        [DisplayName("Nom")]
        public string Nom { get; set; }

        [DisplayName("Type")]
        public TypeMedium? TypeMedium { get; set; }

        public List<KeyValuePair<TypeMedium?, string>> TypeMedia { get; private set; }

        public bool Recherche { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        public IPagedList<Medium> Resultats { get; set; }

        public RechercheMediaViewModel() : this(true)
        {
            InitTypeMedia();
        }

        public RechercheMediaViewModel(bool activeSearch)
        {
            this.Recherche = activeSearch;
            InitTypeMedia();
        }


        private void InitTypeMedia()
        {
            TypeMedia = new List<KeyValuePair<TypeMedium?, string>>();
            TypeMedia.Add(new KeyValuePair<TypeMedium?, string>(null, "Tous"));
            TypeMedia.Add(new KeyValuePair<TypeMedium?, string>(ADA.Domain.Constantes.TypeMedium.Document, "Documents"));
            TypeMedia.Add(new KeyValuePair<TypeMedium?, string>(ADA.Domain.Constantes.TypeMedium.Photo, "Photos"));
            TypeMedia.Add(new KeyValuePair<TypeMedium?, string>(ADA.Domain.Constantes.TypeMedium.Son, "Sons"));
            TypeMedia.Add(new KeyValuePair<TypeMedium?, string>(ADA.Domain.Constantes.TypeMedium.Video, "Vidéos"));
        }

    }
}
