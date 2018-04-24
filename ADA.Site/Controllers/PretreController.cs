using ADA.Data.UnitOfWork;
using ADA.Domain.Pretres;
using ADA.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.IO;
using ADA.Infrastructure.PaginationHandler;
using ADA.Site.Extentions;
using ADA.Domain.Services.Interface;
using ADA.Site.ModelBinder;
using ADA.Domain.Fonctions;

namespace ADA.Site.Controllers
{
    public class PretreController : Controller
    {

        public IUnitOfWork _unitOfWork;
        public IPretreService _pretreService;

        public PretreController(IUnitOfWork unitOfWork, IPretreService pretreService)
        {
            _unitOfWork = unitOfWork;
            _pretreService = pretreService;
        }

        // GET: Pretre
        public ActionResult Index()
        {
            return View("~/Views/Pretre/RechercheParNom/Recherche.cshtml", new RecherchePretreViewModel(false));
        }

        public ActionResult IndexRecherchePretreFonctionLieu()
        {
            var vm = new RecherchePretreParLieuViewModel(false) { FonctionLieuViewModel = GetFonctionLieuViewModel(1) };

            return View("~/Views/Pretre/RechercheParFonctionLieu/Recherche.cshtml", vm);
        }

        public ActionResult Recherche(RecherchePretreViewModel searchParameters)
        {
            Expression<Func<Pretre, bool>> filter = b => (searchParameters.Nom == null || b.Nom.Contains(searchParameters.Nom))
                                                        && (searchParameters.Prenom == null || b.Prenom.Contains(searchParameters.Prenom))
                                                        && (searchParameters.AnneeDeces == null || b.AnneeDeces == searchParameters.AnneeDeces)
                                                        && (searchParameters.AnneeNaissance == null || b.AnneeNaissance == searchParameters.AnneeNaissance)
                                                        && (searchParameters.Commune == null || b.Commune.Nom.Contains(searchParameters.Commune));

            searchParameters.Resultats = _unitOfWork.Pretres.PaginateWithTriModel(new PaginationRequest(searchParameters.Pagination.Valeur, searchParameters.Page),
                filter, searchParameters.Tri, b => b.Photos, b => b.Documents, b => b.ArticlesRevue).ToPagedListMvc(searchParameters.Page, searchParameters.Pagination.Valeur);

            return View("~/Views/Pretre/RechercheParNom/Recherche.cshtml", searchParameters);
        }

        //     public ActionResult Recherche(RecherchePretreViewModel searchParameters)
        //{
        //    Expression<Func<Pretre, bool>> filter = b => (searchParameters.Nom == null || b.Nom.Contains(searchParameters.Nom))
        //                                                && (searchParameters.Prenom == null || b.Prenom.Contains(searchParameters.Prenom))
        //                                                && (searchParameters.AnneeDeces == null || b.AnneeDeces == searchParameters.AnneeDeces)
        //                                                && (searchParameters.AnneeNaissance == null || b.AnneeNaissance == searchParameters.AnneeNaissance)
        //                                                && (searchParameters.Commune == null || b.Commune.Nom.Contains(searchParameters.Commune));

        //    searchParameters.Resultats = _unitOfWork.Pretres.GetPretre(searchParameters.Nom, searchParameters.Prenom, 
        //        searchParameters.AnneeNaissance, searchParameters.AnneeDeces, searchParameters.Commune,
        //        searchParameters.Tri,
        //        new PaginationRequest(searchParameters.Pagination.Valeur, searchParameters.Page)).ToPagedListMvc(searchParameters.Page, searchParameters.Pagination.Valeur);

        //    return View("~/Views/Pretre/RechercheParNom/Recherche.cshtml",searchParameters);
        //}

        public ActionResult RecherchePretreFonctionLieu(RecherchePretreParLieuViewModel rp)
        {

            Expression<Func<Pretre, bool>> filter;
                        
            filter = p => p.FonctionsLieu.Any(fl => (!rp.LieuId.HasValue || fl.Lieu.Id == rp.LieuId)
                && fl.Lieu.TypeLieu.Id == rp.TypeLieuId
                && (!rp.AnneeExercice.HasValue || (fl.AnneeDebut <= rp.AnneeExercice.Value && fl.AnneeFin >= rp.AnneeExercice.Value))
                && (rp.NomLieu == null || fl.Lieu.Nom.Contains(rp.NomLieu))
                && (!rp.FonctionId.HasValue || fl.Fonction.Id == rp.FonctionId.Value));

            rp.FonctionLieuViewModel = GetFonctionLieuViewModel(rp.TypeLieuId, rp.LieuId, rp.NomLieu, rp.FonctionId);
            
            rp.Resultats = _unitOfWork
                .Pretres
                .PaginateWithTriModel(new PaginationRequest(rp.Pagination.Valeur, rp.Page), filter, rp.Tri, 
                            b => b.Photos, 
                            b => b.Documents, 
                            b => b.ArticlesRevue,
                            b => b.FonctionsLieu.Select( fl => fl.Fonction),
                            b => b.FonctionsLieu.Select(fl => fl.Lieu))
                            .ToPagedListMvc(rp.Page, rp.Pagination.Valeur);

            // on affiche que les fonctioLieu relatives à la séléction, pour les lieux et les fonctions
            rp.Resultats.ToList().ForEach(p => p.FonctionsLieu = p.FonctionsLieu.Where(fl => (!rp.LieuId.HasValue || fl.Lieu.Id == rp.LieuId)
                && fl.Lieu.TypeLieu.Id == rp.TypeLieuId
                && (!rp.AnneeExercice.HasValue || (fl.AnneeDebut <= rp.AnneeExercice.Value && fl.AnneeFin >= rp.AnneeExercice.Value))
                && ( fl.Lieu.TypeLieu.RechercheParLieu == true || (rp.NomLieu == null || fl.Lieu.Nom.Contains(rp.NomLieu)))
                && (!rp.FonctionId.HasValue || fl.Fonction.Id == rp.FonctionId.Value)).ToList());

            return View("~/Views/Pretre/RechercheParFonctionLieu/Recherche.cshtml", rp);
        }

        private FonctionLieuViewModel GetFonctionLieuViewModel(int? defautTypeLieuId, int? lieuId = null, string nomLieu = null, int? fonctionId = null)
        {
            var vm = new FonctionLieuViewModel();

            var typesLieux = _unitOfWork.TypeLieu.Get();
            var defautTypeLieu = typesLieux.FirstOrDefault( b => b.Id == defautTypeLieuId);

            vm.TypesLieu = typesLieux;

            if (defautTypeLieu != null && defautTypeLieu.TypeRecherche == Domain.Constantes.TypeRecherche.ListeDeroulante)
            {
                vm.Lieux = _unitOfWork.Lieux.Get(b => b.TypeLieu.Id == defautTypeLieuId);
            }    
            
            if (defautTypeLieu != null)
            {
                    List<Fonction> fonctions = new List<Fonction>();
                    fonctions.AddRange(_unitOfWork.FonctionsTypeLieu.Get(b => b.TypeLieu.Id == defautTypeLieuId.Value, null, b => b.Fonction).Select(b => b.Fonction));
                
                    if (lieuId.HasValue)
                        fonctions.AddRange(_unitOfWork.FonctionsLieu.Get(b => b.Lieu.Id == lieuId.Value, null, b => b.Fonction).Select(b => b.Fonction));

                    fonctions = fonctions.OrderBy( b => b.Nom).ToList();
                    vm.Fonctions = fonctions;

            }

            return vm;
        }

        public ActionResult Fiche(int id, string niceUrl)
        {
            var pretre = _unitOfWork.Pretres.FindBy(id);

            if (pretre == null)
            {
                throw new HttpException(404, "Not found");
            }

            MemoryStream stream = new MemoryStream();

            var fileName = String.Format("{0} {1}.pdf", pretre.Nom, pretre.Prenom);

            _pretreService.CreatePdf(id, stream);
            
            stream.Flush();
            stream.Position = 0; 

            return File(stream, "application/pdf");
        }

        public FileStreamResult Fiches(int[] ids)
        {
            var pretres = _unitOfWork.Pretres.Get(b => ids.Contains(b.Id), null, b => b.Documents);
            MemoryStream stream = new MemoryStream();

            var fileName = String.Format("plusieurs pretres.pdf");

            _pretreService.CreatePdf(ids, stream);

            stream.Flush(); //Always catches me out
            stream.Position = 0; //Not sure if this is required

            return File(stream, "application/pdf");
        }

    }

    
}
