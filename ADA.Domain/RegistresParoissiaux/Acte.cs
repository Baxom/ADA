using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.Domain.Revues;
using ADA.Domain.Lieux;
using System.IO;
using MoreLinq;

namespace ADA.Domain.RegistresParoissiaux
{
    public abstract class Acte : PlageTagDocument
    {
        private const string _codePage = "[NUMERO_PAGE]";
        private const string _codeCodeParoisseRegistre = "[CODE_PAROISSE_REGISTRE]";
        private const string _codeNomParoisseRegistre = "[NOM_PAROISSE_REGISTRE]";
        private const string _codeAnneeRegistre = "[ANNEE_REGISTRE]";
        private const string _codeLibelleActePluriel = "[LIBELLE_ACTE_PLURIEL]";

        public int AnneeRegistreParoissial { get; set; }
        public int ParoisseRegistreId { get; set; }
        public Lieu ParoisseRegistre { get; set; }

        public ListePlage PagesReferences { get; set; }

        public string RegistreParoissial { get { return String.Format("{0}-{1}", ParoisseRegistre.Nom, AnneeRegistreParoissial); } }

        public abstract string Libelle { get; }
        public abstract string LibellePluriel { get; }
        public abstract string Denomination { get; }

        public override string Tag
        {
            get
            {
                return BuildTag(0);
            }
        }

        public override string ShortTag
        {
            get
            {
                return String.Format("{0} - {1}", Libelle, Denomination);
            }
        }

        static string _fichierModel = System.Configuration.ConfigurationManager.AppSettings["FicherRegistreTemplate"];
        static string _dossierModel = System.Configuration.ConfigurationManager.AppSettings["SousRepertoireRegistreTemplate"];
        static string _tagModel = System.Configuration.ConfigurationManager.AppSettings["TagRegistreTemplate"];

        public override IEnumerable<Document> GetDocuments()
        {
            return Pages.ListePages.Select(b => new DocumentActe(this.Tag, BuildNomCompletFichier(b))).DistinctBy( b => b.NomCompletFichier);
        }


        private string BuildTemplate(string chaine, int page)
        {
            var retour = chaine;

            if (string.IsNullOrWhiteSpace(retour)) return String.Empty;

            return retour
                .Replace(_codeCodeParoisseRegistre, this.ParoisseRegistre != null ? this.ParoisseRegistre.Id.ToString() : _codeCodeParoisseRegistre)
                .Replace(_codeAnneeRegistre, this.AnneeRegistreParoissial.ToString())
                .Replace(_codeNomParoisseRegistre, this.ParoisseRegistre != null ? this.ParoisseRegistre.Nom.ToString() : _codeNomParoisseRegistre)
                .Replace(_codeLibelleActePluriel, this.LibellePluriel)
                .Replace(_codePage, page.ToString());
        }

        public string BuildNomCompletFichier(int page)
        {
            var temp1 = BuildTemplate(_dossierModel, page);
            var temp2 = BuildTemplate(_fichierModel, page);

            return Path.Combine(temp1, temp2);
        }

        public string BuildTag(int page)
        {
            return BuildTemplate(_tagModel, page);
        }
    }
}
