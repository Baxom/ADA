using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.Domain.References;

namespace ADA.Domain.Pretres
{
    public class Pretre : EntityBase
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Chanoine { get; set; }
        public int? AnneeNaissance { get; set; }
        public int? AnneeDeces { get; set; }
        public bool Decede { get; set; }

        public string NomComplet { 
            get {
                return String.Format("{0} {1}", Nom, Prenom);
            } 
        }

        public string NomEtDateVie
        {
            get
            {
                return String.Format("{0} ({1})", NomComplet, DateVie);
            }
        }

        public string DateVie
        {
            get
            {
                var anneeNaissanceChaine = AnneeNaissance.HasValue ? AnneeNaissance.ToString() : "?";
                var anneeDecesChaine = AnneeDeces.HasValue ? AnneeDeces.ToString() : Decede ? "?" : "??";

                return String.Format("{0}-{1}", anneeNaissanceChaine, anneeDecesChaine);
            }
        }

        public bool HasDocumentsOuPhotos
        {
            get
            {
                return (this.Documents != null && this.Documents.Any()) || (this.Photos != null && this.Photos.Any()) || (this.ArticlesRevue != null && this.ArticlesRevue.Any());
            }
        }

        public Commune Commune { get; set; }

        public ICollection<PretreArticleRevue> ArticlesRevue { get; set; }
        public ICollection<PretreDocument> Documents { get; set; }
        public ICollection<PretreFonctionLieu> FonctionsLieu { get; set; }
        public ICollection<PretreContextHistorique> ContextHistoriques { get; set; }
        public ICollection<PretrePhoto> Photos { get; set; }
        
        private Pretre ()
	    {
            ArticlesRevue = new List<PretreArticleRevue>();
            Documents = new List<PretreDocument>();
            FonctionsLieu = new List<PretreFonctionLieu>();
            Photos = new List<PretrePhoto>();
            ContextHistoriques = new List<PretreContextHistorique>();
	    }

        public IEnumerable<string> GetFiles()
        {
            return Documents.Select(b => b.NomCompletFichier);
        }

        public IEnumerable<string> GetFormatedFonctions()
        {
            return FonctionsLieu
                .OrderBy( b => b.AnneeDebut)
                .ThenBy( b => b.AnneeFin )
                .ThenBy( b => b.Lieu.Nom )
                .ThenBy( b => b.Fonction.Nom )
                .Select( b => String.Format("{0} de {1} de {2} à {3}", 
                    b.Fonction.Nom, 
                    b.Lieu.Nom, 
                    b.AnneeDebut,
                    b.AnneeFin));
        }

    }
}

