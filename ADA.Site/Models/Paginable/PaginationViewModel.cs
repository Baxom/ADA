using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ADA.Site.Models.Paginable
{
    public class PaginationViewModel
    {
        public int Valeur { get; set; }
        public IEnumerable<int> Valeurs { get; set; }

        public PaginationViewModel()
            : this(Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["PaginationValeurParDefaut"]))
        {
            
        }

        public PaginationViewModel(int valeur)
        {
            Valeur = valeur;
            Valeurs = System.Configuration.ConfigurationManager.AppSettings["PaginationValeurs"].Split('|').Select(b => Int32.Parse(b));
        }


    }
}