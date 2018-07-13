using ADA.Data.UnitOfWork;
using ADA.Site.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ADA.Site
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /* route autocompletion commune  */

            routes.MapRoute(
                name: "AutocompletionCommune",
                url: "reference/commune/auto-complete/{filtre}",
                defaults: new { controller = "Reference", action = "Commune" });

            /* route recherche pretre par lieu/fonction */

            routes.MapRoute(
                name: "IndexRecherchePretreFonctionLieu",
                url: "pretre/lieu-fonction",
                defaults: new { controller = "Pretre", action = "IndexRecherchePretreFonctionLieu" });



            routes.MapRoute(
                name: "RecherchePretreFonctionLieu",
                url: "pretre/lieu-fonction/recherche",
                defaults: new { controller = "Pretre", action = "RecherchePretreFonctionLieu" });

            routes.MapRoute(
                name: "FichePretre",
                url: "pretre/fiche/{id}/{niceUrl}",
                defaults: new { controller = "Pretre", action = "Fiche" });

            /* Route revues */

           routes.MapRoute(
                name: "SelectionArticle",
                url: "revue/selection-article/",
                defaults: new { controller = "Revue", action = "Pdfs" });


            routes.MapRoute(
                name: "RechercheParRevue",
                url: "revue/recherche/{codeRevue}",
                defaults: new { controller = "Revue", action = "Recherche", codeRevue = UrlParameter.Optional });

            routes.MapRoute(
                name: "IndexRechercheParRevue",
                url: "revue/{codeRevue}",
                defaults: new { controller = "Revue", action = "Index", codeRevue = UrlParameter.Optional });


            routes.MapRoute(
                name: "RegistresParoissiaux",
                url: "registres-paroissiaux/{action}/{id}",
                defaults: new { controller = "RegistresParoissiaux", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            routes.MapRoute(
                    "DefaultWithFiltre",
                    "{controller}/{action}/{filtre}", 
                    new { controller = "Home", action = "Index", filtre = UrlParameter.Optional }
                );

            routes.MapRoute(
                    "ImageRoute",
                    "image/thumbnail/{filename}",
                    new { controller = "Image", action = "Thumbnail", filename = "" }
                );
            

        }
    }
}
