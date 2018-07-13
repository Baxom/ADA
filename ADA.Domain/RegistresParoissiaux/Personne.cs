using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.RegistresParoissiaux
{
    public class Personne
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }

        public string NomComplet { 
            get {
                return String.Format("{0} {1}", this.Nom, this.Prenom); 
            } 
        }

        public bool Match(string nom, string prenom)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;

            return (String.IsNullOrWhiteSpace(nom) || String.IsNullOrWhiteSpace(this.Nom) || culture.CompareInfo.IndexOf(this.Nom, nom, CompareOptions.IgnoreCase) >= 0 )
                && (String.IsNullOrWhiteSpace(prenom) || String.IsNullOrWhiteSpace(this.Prenom) || culture.CompareInfo.IndexOf(this.Prenom, prenom, CompareOptions.IgnoreCase) >= 0);
        }
    }
}
