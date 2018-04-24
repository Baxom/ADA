using System.Web;
using System.Web.Optimization;

namespace ADA.Site
{
    public class BundleConfig
    {
        // Pour plus d'informations sur le regroupement, visitez http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-1.12.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-3.4.2.js",
                "~/Scripts/knockout-jqAutocomplete.js",
                "~/Scripts/knockout.mapping-latest.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/external-lib").Include(
                        "~/Libs/multiselect-master/scripts/helper.js",
                        "~/Libs/multiselect-master/scripts/multiselect.core.js",
                        "~/Libs/multiselect-master/scripts/multiselect.js"));

            // Utilisez la version de développement de Modernizr pour le développement et l'apprentissage. Puis, une fois
            // prêt pour la production, utilisez l'outil de génération (bluid) sur http://modernizr.com pour choisir uniquement les tests dont vous avez besoin.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/themes/base/jquery-ui.css",
                        "~/Libs/multiselect-master/styles/multiselect.css",
                        "~/Content/site.css"));
        }
    }
}
