using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Pretres
{
    public class PretreDocument : EntityBase
    {
        public int PretreId { get; set; }
        public string Nom { get; set; }
        public string NomCompletFichier { 
            get {
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings["RepertoireFichePretre"], Nom);
            } 
        }
    }
}
