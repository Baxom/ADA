using ADA.Domain.Contexthistoriques;
using ADA.Domain.Fonctions;
using ADA.Domain.Lieux;
using ADA.Domain.Pretres;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ADA.Site.Models
{
    public class FonctionLieuViewModel
    {
        public IEnumerable<TypeLieu> TypesLieu { get; set; }
        public IEnumerable<Fonction> Fonctions { get; set; }
        public IEnumerable<Lieu> Lieux { get; set; }
        public IEnumerable<ContextHistorique> ContextHistoriques { get; set; }

        public FonctionLieuViewModel()
        {
            TypesLieu = new List<TypeLieu>();
            Fonctions = new List<Fonction>();
            Lieux = new List<Lieu>();
            ContextHistoriques = new List<ContextHistorique>();
        }
        
    }
}
