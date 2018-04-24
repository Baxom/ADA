using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Revues
{
    public class ListePlage
    {
        private char _separatorPlage = ',';

        private string _listePagesTexte;
        public string ListePagesTexte
        {
            get
            {
                return String.Join(_separatorPlage.ToString(), Plages.Select(b => b.PlageTexte));
            }
            set
            {
                _listePagesTexte = value;
                if (!String.IsNullOrWhiteSpace(_listePagesTexte))
                    Plages = _listePagesTexte.Split(_separatorPlage).Select(b => new Plage(b)); 
            }
        }

        public IEnumerable<Plage> Plages { get; set; }

        public ListePlage()
        {
            Plages = new List<Plage>();
        }

    }
}
