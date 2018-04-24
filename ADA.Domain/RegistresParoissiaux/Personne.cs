using System;
using System.Collections.Generic;
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
            return (String.IsNullOrWhiteSpace(nom) || String.IsNullOrWhiteSpace(this.Nom) || this.Nom.Contains(nom))
                && (String.IsNullOrWhiteSpace(prenom) || String.IsNullOrWhiteSpace(this.Prenom) || this.Prenom.Contains(prenom));
        }
    }
}
