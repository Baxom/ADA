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
using ADA.Domain.RegistresParoissiaux;
using ADA.Domain.Constantes;

namespace ADA.Site.Controllers
{
    public class RegistresParoissiauxController : Controller
    {
        IUnitOfWork _unitOfWork;

        public RegistresParoissiauxController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Revue
        public ActionResult Index()
        {
            var titi = _unitOfWork
                .Actes
                .Get(b => b.ParoisseRegistreId == 2103, 
                b => b.OrderBy( o => (o is Bapteme) ? (o as Bapteme).Nom : 
                                        (o is Sepulture) ? (o as Sepulture).Nom : 
                                        (o as Mariage).Epoux.Nom)
                                        .ThenBy( o => (o is Bapteme) ? (o as Bapteme).Prenom : 
                                        (o is Sepulture) ? (o as Sepulture).Prenom :
                                        (o as Mariage).Epoux.Prenom)
                ).ToList();


            return View("~/Views/RegistresParoissiaux/Recherche.cshtml", new RegistresParoissiauxViewModel(false));
        }

        public ActionResult Recherche(RegistresParoissiauxViewModel model)
        {

            string nom = model.Nom ?? String.Empty;
            string prenom = model.Prenom ?? String.Empty;

            Expression<Func<Acte, bool>> filter = b =>
                (model.AnneeRegistre == null || b.AnneeRegistreParoissial == model.AnneeRegistre )
                && (model.ParoisseRegistre == null || (b.ParoisseRegistre.Nom.Contains(model.ParoisseRegistre) 
                && b.ParoisseRegistre.TypeLieu.TypeFonctionnel == TypeLieuFonctionnel.Paroisse))
                && (
                (((b is Mariage) && (!model.TypeActe.HasValue || model.TypeActe.Value == TypeActe.Mariage)) 
                && (((b as Mariage).Epoux.Nom.Contains(nom) && (b as Mariage).Epoux.Prenom.Contains(prenom))
                    || ((b as Mariage).Epouse.Nom.Contains(nom) && (b as Mariage).Epouse.Prenom.Contains(prenom)))
                && ((!model.DateDeMariageCivil.HasValue) || (b as Mariage).DateMariageCivil == model.DateDeMariageCivil)
                )

                || (((b is Bapteme) && (!model.TypeActe.HasValue || model.TypeActe.Value == TypeActe.Bapteme))
                && ((b as Bapteme).Nom.Contains(nom) && (b as Bapteme).Prenom.Contains(prenom))
                && ((!model.DateDeNaissance.HasValue) || (b as Bapteme).DateNaissance == model.DateDeNaissance)
                )

                || (((b is Sepulture) && (!model.TypeActe.HasValue || model.TypeActe.Value == TypeActe.Sepulture))
                && ((b as Sepulture).Nom.Contains(nom) && (b as Sepulture).Prenom.Contains(prenom))
                && ((!model.DateDeDeces.HasValue) || (b as Sepulture).DateDeces == model.DateDeDeces)
                )  
                );
            
            model.Resultats = _unitOfWork
                .Actes
                .PaginateWithTriModel(new PaginationRequest(model.Pagination.Valeur, model.Page), filter, model.Tri, b => b.ParoisseRegistre)
                .ToPagedListMvc(model.Page, model.Pagination.Valeur);

            return View("~/Views/RegistresParoissiaux/Recherche.cshtml", model);
        }

    }
}