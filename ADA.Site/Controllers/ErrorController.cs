using ADA.Data.UnitOfWork;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADA.Site.Controllers
{
    public class ErrorController : Controller
    {

        public ErrorController(ILogger logger, IUnitOfWork unitOfWork) 
        {
        }

        public ActionResult NotFound()
        {
            return View("~/views/error/Erreur_404.cshtml");
        }

    }
}