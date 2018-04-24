using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.Domain.Indexation;

namespace ADA.Domain.Revues
{
    public class Revue : EntityBase
    {
        private const string _codePage = "[NUMERO_PAGE]";
        private const string _codePeriodePublication = "[PERIODE_PUBLICATION]";
        private const string _codeCodeRevue = "[CODE_REVUE]";
        private const string _codeNomRevue = "[NOM_REVUE]";
        private const string _codeCodeRevueMere = "[CODE_REVUE_MERE]";

        public string Code { get; set; }
        public string Nom { get; set; }

        public string NomDossierModele { get; set; }
        public string NomFichierModele { get; set; }
        public string TagModel { get; set; }

        public Revue RevueMere { get; set; }
        public ICollection<Revue> RevuesFille { get; set; }

        public bool RechercheParAnnee { get; set; }
        public bool RechercheParParoisse { get; set; }
        public bool Active { get; set; }
        public bool RechercheDirecte { get; set; }
        public bool RecherchePartielleDepuisGlobale { get; set; }
        public bool RecherchePartielleDepuisGlobaleDefaut { get; set; }

        public ICollection<RevueGroupeIndex> RevueGroupeIndex { get; set; }
        public ICollection<PeriodesRevueLieu> PeriodesParoisses { get; set; }

        public Revue()
        {
            RevuesFille = new List<Revue>();
            PeriodesParoisses = new List<PeriodesRevueLieu>();
            RevueGroupeIndex = new List<RevueGroupeIndex>();
        }

        private string BuildTemplate(string chaine, string periodePublication, int page)
        {
            var retour = chaine;

            if (string.IsNullOrWhiteSpace(retour)) return String.Empty;

            return retour
                .Replace(_codeCodeRevueMere, this.RevueMere != null ? this.RevueMere.Code : "[CODE_REVUE_MERE]")
                .Replace(_codeCodeRevue, this.Code)
                .Replace(_codeNomRevue, this.Nom)
                .Replace(_codePeriodePublication, periodePublication)
                .Replace(_codePage, page.ToString());
        }

        public string BuildNomCompletFichier(int page, string periodePublication)
        {
            return Path.Combine(BuildTemplate(NomDossierModele, periodePublication, page), BuildTemplate(NomFichierModele, periodePublication, page));
        }

        public string BuildTag(int page, string periodePublication)
        {
            return BuildTemplate(TagModel, periodePublication, page);
        }
    }
}
