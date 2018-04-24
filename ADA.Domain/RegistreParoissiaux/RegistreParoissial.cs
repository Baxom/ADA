using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.Domain.Lieux;

namespace ADA.Domain.RegistreParoissiaux
{
    public class RegistreParoissial : EntityBase
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime? DateNaissance { get; set; }

        public Lieu Paroisse { get; set; }

        public TypeActe Type { get; set; }
    }
}
