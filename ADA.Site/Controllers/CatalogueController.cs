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
using ADA.Domain.Catalogues;
using System.IO;
using ADA.Domain.Services.Interface;
using ADA.Infrastructure.Services.Interface.WordSearchParser;
using ADA.Data.SqlServer.Interface;

namespace ADA.Site.Controllers
{
    public class CatalogueController : Controller
    {
        IUnitOfWork _unitOfWork;
        ICatalogueService _catalogueService;
        IWordSearchParser _wordSearchparser;

        public CatalogueController(IUnitOfWork unitOfWork, ICatalogueService catalogueService, IWordSearchParser wordSearchparser, IFTSStringProvider fTSStringProvider)
        {
            _unitOfWork = unitOfWork;
            _catalogueService = catalogueService;
            _wordSearchparser = wordSearchparser;

        }

        // GET: Revue
        public ActionResult Index()
        {
            var model = new RechercheCatalogueViewModel(false);

            initModel(model);

            return View("~/Views/Catalogue/Recherche.cshtml", model);
        }

        private void initModel(RechercheCatalogueViewModel model)
        {
            model.Series = _unitOfWork.Series.Get(includeProperties: b => b.SousSeries);

            if(model.SousSerieId.HasValue)
            {
                model.SousSerie = model.Series.SelectMany(s => s.SousSeries).SingleOrDefault(ss => ss.Id == model.SousSerieId.Value);

                if (model.SousSerie == null) model.SousSerieId = null;
            }

            model.MarkableElement = _wordSearchparser.Parse(model.Titre).ToList();
        }

        public ActionResult Recherche(RechercheCatalogueViewModel model)
        {
            var titre = _unitOfWork.FTSContains(model.Titre);
            Expression<Func<Catalogue, bool>> filter = b => (model.Titre == null || b.Titre.Contains(titre))
                                                    && (!model.SousSerieId.HasValue || b.SousSerie.Id == model.SousSerieId.Value)
                                                    && (!model.SerieId.HasValue || b.SousSerie.SerieId == model.SerieId.Value);
            initModel(model);
            model.Resultats = _unitOfWork.Catalogues.Paginate(new PaginationRequest(model.Pagination.Valeur, model.Page), filter).ToPagedListMvc(model.Page, model.Pagination.Valeur);

            return View("~/Views/Catalogue/Recherche.cshtml", model);
        }

        public ActionResult Pdf(int id, string niceUrl, string searchTerms)
        {
            var catalogue = _unitOfWork.Catalogues.FindBy(id);

            if (catalogue == null)
            {
                throw new HttpException(404, "Not found");
            }

            MemoryStream stream = new MemoryStream();
            
            _catalogueService.CreatePdf(id, searchTerms, stream);

            stream.Flush();
            stream.Position = 0;

            return File(stream, "application/pdf");
        }

        public FileStreamResult Pdfs(int[] ids, string searchTerms)
        {
            MemoryStream stream = new MemoryStream();

            var fileName = String.Format("plusieurs pretres.pdf");

            _catalogueService.CreatePdf(ids, searchTerms, stream);

            stream.Flush(); //Always catches me out
            stream.Position = 0; //Not sure if this is required

            return File(stream, "application/pdf");
        }

    }
}
