using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.Domain.Indexation;
using System.Text.RegularExpressions;

namespace ADA.Domain.Revues
{
    public class Revue : EntityBase
    {
        public delegate string RevueMatchEvaluator(Match match, string periodePublication, int page, int? numeroRevue, Revue revue);

        private const string _codePage = "NUMERO_PAGE";
        private const string _codePeriodePublication = "PERIODE_PUBLICATION";
        private const string _codeCodeRevue = "CODE_REVUE";
        private const string _codeNomRevue = "NOM_REVUE";
        private const string _codeNumeroRevue = "NUMERO_REVUE";
        private const string _codeCodeRevueMere = "CODE_REVUE_MERE";

        private Dictionary<string, object> _dctCodeValue;

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

        private Regex regExCurlyBrakets = new Regex(@"{[A-Za-z_]+}");
        private Regex regExConditionalTag = new Regex(@"\(\?{[A-Za-z_]+}.*\?\)");


        public Revue()
        {
            RevuesFille = new List<Revue>();
            PeriodesParoisses = new List<PeriodesRevueLieu>();
            RevueGroupeIndex = new List<RevueGroupeIndex>();
        }

        private string BuildTemplate(string chaine, string periodePublication, int page, int? numeroRevue)
        {
            var templateReplaceTag = "[{0}]";
            if (string.IsNullOrWhiteSpace(chaine)) return String.Empty;
            var retour = DeleteCondionalTag(chaine, periodePublication, page, numeroRevue);
            if (string.IsNullOrWhiteSpace(retour)) return String.Empty;

            return retour
                .Replace(String.Format(templateReplaceTag, _codeCodeRevueMere), this.RevueMere != null ? this.RevueMere.Code : String.Format(templateReplaceTag, _codeCodeRevueMere))
                .Replace(String.Format(templateReplaceTag, _codeCodeRevue), this.Code)
                .Replace(String.Format(templateReplaceTag, _codeNomRevue), this.Nom)
                .Replace(String.Format(templateReplaceTag, _codePeriodePublication), periodePublication)
                .Replace(String.Format(templateReplaceTag, _codeNumeroRevue), numeroRevue.HasValue ? numeroRevue.Value.ToString() : String.Empty)
                .Replace(String.Format(templateReplaceTag, _codePage), page.ToString());
        }

        private RevueMatchEvaluator evaluatorConditionalTags = delegate(Match match, string periodePublication, int page, int? numeroRevue, Revue revue)
        {
            Regex regExCurlyBrakets = new Regex(@"{[A-Za-z_]+}");
        var matchTagCondition = regExCurlyBrakets.Match(match.Value);

        var code = matchTagCondition.Value.Replace("{", "").Replace("}", "");

            bool keepConditionalTag = !ConditionalIsNullOrEmpty(code, periodePublication, page, numeroRevue, revue);

            if (keepConditionalTag) {
                return match.Value.Replace(String.Format("(?{{{0}}}", code), String.Empty).Replace("?)", String.Empty);
            }
            else
            {
                return String.Empty;
            }
        };

        private static bool ConditionalIsNullOrEmpty(string code, string periodePublication, int page, int? numeroRevue, Revue revue)
        {
            switch(code)
            {
                case _codePage: return false;
                case _codePeriodePublication: return String.IsNullOrEmpty(periodePublication);
                case _codeCodeRevue: return String.IsNullOrEmpty(revue.Code);
                case _codeNomRevue: return String.IsNullOrEmpty(revue.Nom);
                case _codeNumeroRevue: return !numeroRevue.HasValue;
                case _codeCodeRevueMere: return revue.RevueMere == null ? true : String.IsNullOrEmpty(revue.RevueMere.Code);

                default: return true;
            }
                
        }

        private string DeleteCondionalTag(string chaine, string periodePublication, int page, int? numeroRevue)
        {
            return regExConditionalTag.Replace(chaine, match => this.evaluatorConditionalTags(match, periodePublication, page, numeroRevue, this));
        }

        public string BuildNomCompletFichier(int page, string periodePublication, int? numeroRevue)
        {
            return Path.Combine(BuildTemplate(NomDossierModele, periodePublication, page, numeroRevue), BuildTemplate(NomFichierModele, periodePublication, page, numeroRevue));
        }

        public string BuildTag(int page, string periodePublication, int? numeroRevue)
        {
            return BuildTemplate(TagModel, periodePublication, page, numeroRevue);
        }
    }
}
