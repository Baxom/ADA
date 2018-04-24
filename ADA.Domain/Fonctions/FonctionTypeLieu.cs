using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.Domain.Constantes;
using ADA.Domain.Lieux;

namespace ADA.Domain.Fonctions
{
    public class FonctionTypeLieu : EntityBase
    {
        public TypeLieu TypeLieu { get; set; }
        public Fonction Fonction { get; set; }

    }
}
