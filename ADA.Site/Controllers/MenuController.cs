using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADA.Data.UnitOfWork;
using System.Web.Mvc;

namespace ADA.Site.Controllers
{
    public class MenuController : Controller
    {
        readonly IUnitOfWork _unitOfWork;

        public MenuController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult MainMenu()
        {
            var revues = _unitOfWork.Revues.Get( b => b.Active && b.RechercheDirecte).ToList();

            return View(revues);
        }
    }
}