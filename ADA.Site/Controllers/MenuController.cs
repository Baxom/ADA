using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ADA.Data.UnitOfWork;
using System.Web.Mvc;
using ADA.Site.Models.Menu;

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
            var vm = new MenuViewModel();
            vm.Revues = _unitOfWork.Revues.Get( b => b.Active && b.RechercheDirecte).ToList();
            vm.Fonds = _unitOfWork.Fonds.Get(b => b.Actif && b.FondPere == null, null, b => b.FondsFils).ToList();
            
            return View(vm);
        }
    }
}
