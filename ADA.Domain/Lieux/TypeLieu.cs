using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.Domain.Constantes;

namespace ADA.Domain.Lieux
{
    public class TypeLieu : EntityBase
    {
        public TypeRecherche TypeRecherche { get; set; }
        public string Nom { get; set; }
        public TypeLieuFonctionnel? TypeFonctionnel { get; set; }

    }
}
