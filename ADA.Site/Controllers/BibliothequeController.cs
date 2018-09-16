using ADA.Data.UnitOfWork;
using ADA.Infrastructure.PaginationHandler;
using ADA.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ADA.Site.Extentions;
using ADA.Site.Models.Index;
using ADA.Domain.Revues;
using System.Linq.Expressions;
using ADA.Domain.Bibliotheques;
using ADA.Infrastructure.Services.Interface.WordSearchParser;

namespace ADA.Site.Controllers
{
    public class BibliothequeController : Controller
    {
        IUnitOfWork _unitOfWork;

        public BibliothequeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Revue
        public ActionResult Index()
        {
            return View("~/Views/Bibliotheque/Recherche.cshtml", new RechercheBibliothequeViewModel(false));
        }

       
        public ActionResult Recherche(RechercheBibliothequeViewModel model)
        {
            var titre = _unitOfWork.FTSContains(model.Nom);

            Expression<Func<Bibliotheque, bool>> filter = b =>
                (model.Auteur == null || b.Auteur.Contains(model.Auteur))
                && (model.Nom == null || b.Titre.Contains(titre));
            model.Resultats = _unitOfWork.Bibliotheques.Paginate(new PaginationRequest(model.Pagination.Valeur, model.Page), filter).ToPagedListMvc(model.Page, model.Pagination.Valeur);

            return View("~/Views/Bibliotheque/Recherche.cshtml", model);
        }

    }
}