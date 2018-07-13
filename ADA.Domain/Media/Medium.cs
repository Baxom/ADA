using ADA.Domain.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADA.Domain.References;
using ADA.Domain.Constantes;

namespace ADA.Domain.Media
{
    public class Medium : EntityBase
    {
        public string Titre { get; set; }
        public TypeMedium Type { get; set; }
        public string TagsString { get; set; }
        public string FileName { get; set; }
        public string ThumbNail { get; set; }
        
        public string RepertoireNomThumbnail
        {
            get
            {
                if (string.IsNullOrEmpty(ThumbNail)) return null;

                return System.IO.Path.Combine(System.Configuration.ConfigurationManager.AppSettings["RepertoireThumbnailMedia"], ThumbNail);
            }
        }

        public string RepertoireNom
        {
            get
            {
                return MediaFileLocator.GetLocation(this);
            }
        }

        public ICollection<MediumTag> Tags { get; set; }

        protected Medium()
	    {
            Tags = new List<MediumTag>();
	    }

    }
}

