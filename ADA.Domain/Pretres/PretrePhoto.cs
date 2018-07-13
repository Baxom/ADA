using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Pretres
{
    public class PretrePhoto : EntityBase
    {
        public int PretreId { get; set; }
        public string NomFichier { get; set; }

        public string RepertoireNomFichier
        {
            get
            {
                return System.IO.Path.Combine(System.Configuration.ConfigurationManager.AppSettings["RepertoirePhotoPretre"], NomFichier);
            }
        }

        public string NomCompletFichier { 
            get {
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,Path.Combine(System.Configuration.ConfigurationManager.AppSettings["RepertoirePhotoPretre"], NomFichier));
            } 
        }
    }
}
