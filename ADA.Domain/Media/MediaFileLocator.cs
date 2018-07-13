using System;

namespace ADA.Domain.Media
{
    internal static class MediaFileLocator
    {
        public static string GetLocation(Medium medium)
        {
            var localFolder = "";

            if(medium is Fonds.FondMedium)
            {
                localFolder = (medium as Fonds.FondMedium).Fond.Repertoire;
            }
            else
            {

                switch (medium.Type)
                {
                    case Constantes.TypeMedium.Document: localFolder = System.Configuration.ConfigurationManager.AppSettings["RepertoireMediaDocument"]; break;
                    case Constantes.TypeMedium.Video: localFolder = System.Configuration.ConfigurationManager.AppSettings["RepertoireMediaVideo"]; break;
                    case Constantes.TypeMedium.Photo: localFolder = System.Configuration.ConfigurationManager.AppSettings["RepertoireMediaPhoto"]; break;
                    case Constantes.TypeMedium.Son: localFolder = System.Configuration.ConfigurationManager.AppSettings["RepertoireMediaSon"]; break;

                    default: throw new NotImplementedException();
                }

            }

            return System.IO.Path.Combine(localFolder, medium.FileName);
        }
    }
}
