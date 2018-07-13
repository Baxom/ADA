using ADA.Domain.Base;
using ADA.Domain.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Fonds
{
    public class InformationFondMedium : EntityBase
    {
        public InformationFond InformationFond { get; set; }
        public string ValueString { get; set; }
        public int? ValueInt { get; set; }
        public DateTime? ValueDate { get; set; }
        
        public string GetStringValue()
        {
            switch (InformationFond.Type)
            {
                case TypeColonneFond.Chaine: return !String.IsNullOrEmpty(ValueString) ? ValueString : String.Empty;
                case TypeColonneFond.Date: return ValueDate.HasValue ? ValueDate.Value.ToString("dd/MM/yyyy") : String.Empty;
                case TypeColonneFond.Entier: return ValueInt.HasValue ? ValueInt.Value.ToString() : String.Empty;
                default: throw new Exception("Type colonne introuvable");
                        
            }
        }
    }
}
