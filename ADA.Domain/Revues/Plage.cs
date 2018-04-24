using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Revues
{
    public class Plage
    {
        
        public int PageDebut { get; set; }
        public int PageFin { get; set; }

        public IEnumerable<int> Pages
        {
            get
            {

                var resultat = new List<int>();
                if (PageFin < PageDebut) return resultat; // TODO exception ?
                else
                {
                    var debut = PageDebut;

                    while(debut <= PageFin) {
                        resultat.Add(debut);
                        debut++;
                    }
                }

                return resultat;
            }
        }

        public string PlageTexte
        {
            get
            {
                return PageDebut == PageFin ? PageDebut.ToString() : String.Format("{0}-{1}", PageDebut.ToString(), PageFin.ToString());
            }
        }

        private Plage()
        {

        }

        public Plage(string plageTexte) : this()
        {
            int pageDebut = -1;
            int pageFin = -1;

            if (plageTexte.Contains("-"))
            {
                var pageStrings = plageTexte.Split('-');

                if (pageStrings.Length != 2) return;

                if(!Int32.TryParse(pageStrings[0], out pageDebut)) return;
                if (!Int32.TryParse(pageStrings[1], out pageFin)) return;
            }
            else
            {
                if (!Int32.TryParse(plageTexte, out pageDebut)) return;
                pageFin = pageDebut;
            }

            PageDebut = pageDebut;
            PageFin = pageFin;
        }
    }
}
