using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ADA.Site.App_Start;

namespace ADA.Site.Filter
{
    public class ErrorFilter : System.Web.Mvc.HandleErrorAttribute
    {
        public ILogger Logger { get { return WindsorActivator.bootstrapper.Container.Resolve<ILogger>(); } }

        public override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;
            string controller = "";
            string action = "";
            bool is404 = false;

            if (exception.GetType() == typeof(HttpException))
            {
                is404 = (((HttpException)exception).GetHttpCode() == 404);
            }

            controller = filterContext.RouteData.Values["controller"].ToString();
            action = filterContext.RouteData.Values["action"].ToString();
            
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            else
            {
                var methodHttp = filterContext.RequestContext.HttpContext.Request.HttpMethod;

                var expectedAttribute = methodHttp == "POST" ?  typeof(HttpPostAttribute) : null;

                //Determine the return type of the action
                string actionName = filterContext.RouteData.Values["action"].ToString();
                Type controllerType = filterContext.Controller.GetType();
                var method = controllerType.GetMethods().Where(b => b.Name.Equals(actionName, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    //&& (expectedAttribute == null
                    //|| b.GetCustomAttributes(true).Any(c => c as HttpPostAttribute != null))).FirstOrDefault();
                
                if(!is404) Logger.Error(String.Format("Erreur dans controlleur {0}, action {1}", 
                    filterContext.Controller.GetType().Name,
                    actionName
                    ), filterContext.Exception);

                var returnType = method.ReturnType;

                var model = new HandleErrorInfo(filterContext.Exception, controllerType.Name, actionName);

                

                //If the action that generated the exception returns JSON
                if (returnType.Equals(typeof(JsonResult)))
                {
                    if (is404)
                    {
                        filterContext.Result = new HttpNotFoundResult();
                    }
                    else
                    {
                        filterContext.Result = new JsonResult()
                        {
                        
                            Data = "Erreur interne"
                        };
                    }
                    
                }

                //If the action that generated the exception returns a view
                if (returnType.Equals(typeof(ActionResult))
                || (returnType).IsSubclassOf(typeof(ActionResult)))
                {
                    if (is404) {
                        filterContext.Result = new ViewResult
                        {
                            ViewName = "~/views/error/Erreur_404.cshtml"
                        };
                    }
                    else
                    {
                        filterContext.Result = new ViewResult
                        {
                            ViewName = "~/views/error/Erreur_500.cshtml",
                            ViewData = new ViewDataDictionary(model)
                        };
                    }
                    
                }


            }

            //Make sure that we mark the exception as handled
            filterContext.ExceptionHandled = true;
        }
    }
}