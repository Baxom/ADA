using ADA.Domain.Base;
using ADA.Domain.Indexation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Catalogues
{
    public class Catalogue : EntityBase
    {
        public string Cote { get; set; }
        public string Titre { get; set; }
        public string Date { get; set; }

        public SousSerie SousSerie { get; set; } 
    }
}
