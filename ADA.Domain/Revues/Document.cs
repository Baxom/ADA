using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Revues
{
    public class Document
    {
        protected string _basePath = "";

        public string Tag { get; set; }
        public string NomFichier { get; set; }
        public virtual string NomCompletFichier
        {
            get
            {
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _basePath, NomFichier);
            }
        }

        public Document(string tag, string nomFichier, string basePath)
        {
            Tag = tag;
            NomFichier = nomFichier;
            _basePath = basePath;
        }
    }
}
