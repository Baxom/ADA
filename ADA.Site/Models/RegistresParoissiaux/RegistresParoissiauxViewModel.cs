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
using ADA.Domain.RegistresParoissiaux;
using ADA.Domain.Constantes;
using System.ComponentModel.DataAnnotations;

namespace ADA.Site.Models
{
    public class RegistresParoissiauxViewModel : PaginableTriableViewModel, ISearchable
    {

        [DisplayName("Nom")]
        public string Nom { get; set; }

        [DisplayName("Prénom")]
        public string Prenom { get; set; }

        [DisplayName("Année")]
        public int? AnneeRegistre { get; set; }

        [DisplayName("Paroisse")]
        public string ParoisseRegistre { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DisplayName("Date de naissance")]
        public DateTime? DateDeNaissance { get; set; }

        [DisplayName("Date de décès")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateDeDeces { get; set; }

        [DisplayName("Date de mariage")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateDeMariageCivil { get; set; }

        [DisplayName("Type d'acte")]
        public TypeActe? TypeActe { get; set; }

        public List<KeyValuePair<TypeActe?, string>> TypeActes { get; private set; }

        public bool Recherche { get; set; }

        [System.Runtime.Serialization.IgnoreDataMember]
        public IPagedList<Acte> Resultats { get; set; }

        public RegistresParoissiauxViewModel() : this(true)
        {
            
        }

        public RegistresParoissiauxViewModel(bool activeSearch)
        {
            this.Recherche = activeSearch;

            InitTris();
            InitTypeActes();
        }

        protected override void InitTris()
        {

            AddTri<Acte>("Nom prénom croissant", b => b.OrderBy(o => (o is Bapteme) ? (o as Bapteme).Nom :
                                        (o is Sepulture) ? (o as Sepulture).Nom :
                                        ( ((this.Nom != null) && (o as Mariage).Epouse.Nom.Contains(this.Nom)) || ((this.Prenom != null) && (o as Mariage).Epouse.Prenom.Contains(this.Prenom)) ?
                                            (o as Mariage).Epouse.Nom : (o as Mariage).Epoux.Nom))
                                        .ThenBy(o => (o is Bapteme) ? (o as Bapteme).Prenom :
                                        (o is Sepulture) ? (o as Sepulture).Prenom :
                                        (((this.Nom != null) && (o as Mariage).Epouse.Nom.Contains(this.Nom)) || ((this.Prenom != null) && (o as Mariage).Epouse.Prenom.Contains(this.Prenom)) ?
                                            (o as Mariage).Epouse.Prenom : (o as Mariage).Epoux.Prenom)));


            AddTri<Acte>("Nom prénom décroissant", b => b.OrderByDescending(o => (o is Bapteme) ? (o as Bapteme).Nom :
                                        (o is Sepulture) ? (o as Sepulture).Nom :
                                        (((this.Nom != null) && (o as Mariage).Epouse.Nom.Contains(this.Nom)) || ((this.Prenom != null) && (o as Mariage).Epouse.Prenom.Contains(this.Prenom)) ?
                                            (o as Mariage).Epouse.Nom : (o as Mariage).Epoux.Nom))
                                        .ThenByDescending(o => (o is Bapteme) ? (o as Bapteme).Prenom :
                                        (o is Sepulture) ? (o as Sepulture).Prenom :
                                        (((this.Nom != null) && (o as Mariage).Epouse.Nom.Contains(this.Nom)) || ((this.Prenom != null) && (o as Mariage).Epouse.Prenom.Contains(this.Prenom)) ?
                                            (o as Mariage).Epouse.Prenom : (o as Mariage).Epoux.Prenom)));

            AddTri<Acte>("Registre paroissial croissant", o => o.OrderBy(p => p.ParoisseRegistre.Nom + " - " + p.AnneeRegistreParoissial));
        AddTri<Acte>("Registre paroissial décroissant", o => o.OrderByDescending(p => p.ParoisseRegistre.Nom + " - " + p.AnneeRegistreParoissial));
        AddTri<Acte>("Page croissante", o => o.OrderBy(p => p.Pages.ListePagesTexte));
        AddTri<Acte>("Page décroissante", o => o.OrderByDescending(p => p.Pages.ListePagesTexte));


        Tri = Tris.First();
     
        }

        private void InitTypeActes()
        {
            TypeActes = new List<KeyValuePair<TypeActe?, string>>();
            TypeActes.Add(new KeyValuePair<TypeActe?, string>(null, "Tous"));
            TypeActes.Add(new KeyValuePair<TypeActe?, string>(ADA.Domain.Constantes.TypeActe.Bapteme, "Baptêmes"));
            TypeActes.Add(new KeyValuePair<TypeActe?, string>(ADA.Domain.Constantes.TypeActe.Sepulture, "Décès"));
            TypeActes.Add(new KeyValuePair<TypeActe?, string>(ADA.Domain.Constantes.TypeActe.Mariage, "Mariages"));
        }
     
    }
}
