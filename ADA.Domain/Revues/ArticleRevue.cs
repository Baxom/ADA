using ADA.Domain.Base;
using ADA.Domain.Indexation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Revues
{
    public class ArticleRevue : PlageRevue
    {
        public string Titre { get; set; }
        public string Auteur { get; set; }

        public ListePlage PagesReferences { get; set; }

        public ICollection<ArticleRevueIndex> ArticleRevueIndex { get; set; }

        public ArticleRevue()
        {
            PagesReferences = new ListePlage();
        }
    }
}
