using ADA.Domain.Base;
using ADA.Domain.Lieux;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Fonctions
{
    public class FonctionLieu : EntityBase
    {
        public Lieu Lieu { get; set; }
        public Fonction Fonction { get; set; }
    }
}
