using ADA.Site.App_Start;
using ADA.Site.Plumbing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace ADA.Site
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // DI
            config.Services.Replace(typeof(IHttpControllerActivator), new WindsorControllerActivator(WindsorActivator.bootstrapper.Container));

            // Configuration et services API Web

            // Itinéraires de l'API Web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
               name: "InlineContentApi",
               routeTemplate: "api/{controller}/{action}/{id}/{niceUrl}",
               defaults: new { id = RouteParameter.Optional }
           );

            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
