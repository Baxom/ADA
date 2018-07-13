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
using System.IO;
using ADA.Domain.Services.Interface;
using ADA.Domain.Constantes;

namespace ADA.Site.Controllers
{
    public class RevueController : Controller
    {
        IUnitOfWork _unitOfWork;
        IArticleRevueService _revueService;

        public RevueController(IUnitOfWork unitOfWork, IArticleRevueService revueService)
        {
            _unitOfWork = unitOfWork;
            _revueService = revueService;
        }

        // GET: Revue
        public ActionResult Index(RechercheRevueViewModel model)
        {
            model.Recherche = false;
            var revue = GetRevue(model.CodeRevue);

            if (revue == null && !String.IsNullOrEmpty(model.CodeRevue))
            {
                throw new HttpException(404, "Not found");
            }

            BuildModel(model, revue);
            model.RevuesSelectionnees = model.RevuesDisponibles.Where(b => b.RecherchePartielleDepuisGlobaleDefaut).Select(b => b.Id).ToList();

            return View("~/Views/Revue/Recherche.cshtml", model);
        }

        private Domain.Revues.Revue GetRevue(string codeRevue)
        {
            var revue = _unitOfWork.Revues.Get(b => b.Code == codeRevue,
                null, 
                b => b.RevueGroupeIndex.Select(rgi => rgi.GroupeIndex.Index),
                b => b.RevuesFille).FirstOrDefault();

            return revue;
        }

        private IEnumerable<Domain.Revues.Revue> GetRevue(IEnumerable<int> ids)
        {
            var revues = _unitOfWork.Revues.Get(b => ids.Contains(b.Id),
                null,
                b => b.RevuesFille);

            return revues;
        }


        private IEnumerable<GroupeIndexViewModel> GetGroupeIndex(ADA.Domain.Revues.Revue revue)
        {

            List<GroupeIndexViewModel> giv = new List<GroupeIndexViewModel>();

            if (revue == null) return giv;

            revue.RevueGroupeIndex.Select(b => b.GroupeIndex).ToList().ForEach(b => {

                var groupe = new GroupeIndexViewModel();
                groupe.Id = b.Id;
                groupe.Libelle = b.Libelle;

                List<IndexViewModel> index = new List<IndexViewModel>();

                index.Add(new IndexViewModel() { Id = null, Libelle = "Tous" });
                index.AddRange( b.Index.Select( i => new IndexViewModel() { Id = i.Id, Libelle =  i.Libelle}));
                groupe.Index = index;

                giv.Add(groupe);
            });

            return giv;
        }

        public ActionResult Recherche(RechercheRevueViewModel model)
        {
            var revue = GetRevue(model.CodeRevue);

            if (revue == null && !model.RechercheGlobale)
            {
                throw new HttpException(404, "Not found");
            }

            BuildModel(model, revue);

            List<int> selectedIndex = model.SelectedIndex.Values.Where(b => b.HasValue).ToList().Cast<int>().ToList();
            List<int> selectedRevue = null;


            if (model.RechercheGlobale)
            {
                var revues = GetRevue(model.RevuesSelectionnees);

                selectedRevue = revues.Select( b => b.Id).Union( revues.SelectMany( r => r.RevuesFille.Select( rf => rf.Id))).Distinct().ToList();
            }
            else { selectedRevue = new List<int>() { revue.Id }.Union(revue.RevuesFille.Select(rf => rf.Id)).ToList(); }


            Expression<Func<ArticleRevue, bool>> filter = b =>
                selectedRevue.Contains(b.Revue.Id)
                && (!b.Revue.RechercheParAnnee || 
                    ((!model.AnneeDebut.HasValue || model.AnneeDebut.Value <= b.DebutPublication) 
                    && (!model.AnneeFin.HasValue || model.AnneeFin.Value >= b.FinPublication )))
                && (model.Auteur == null || b.Auteur.Contains(model.Auteur))
                && (model.Nom == null || b.Titre.Contains(model.Nom))

                && ((!b.Revue.PeriodesParoisses.Any() || model.Paroisse == null) 
                    || (
                        b.Revue.PeriodesParoisses.Any(pp => pp.Lieu.Nom == model.Paroisse
                            && pp.Lieu.TypeLieu.TypeFonctionnel == TypeLieuFonctionnel.Paroisse
                            && pp.AnneeDebutPeriode <= b.DebutPublication
                            && pp.AnneeFinPeriode >= b.FinPublication
                            )
                    ))
                && (!selectedIndex.Any() || b.ArticleRevueIndex.Join(selectedIndex, outerKey => outerKey.Index.Id, innerKey => innerKey, (o, i) => new { }).Count() == selectedIndex.Count);
            
            model.Resultats = _unitOfWork
                .ArticlesRevue
                .Paginate(new PaginationRequest(model.Pagination.Valeur, model.Page), filter).ToPagedListMvc(model.Page, model.Pagination.Valeur);

            return View("~/Views/Revue/Recherche.cshtml", model);
        }

        private void BuildModel(RechercheRevueViewModel model, Revue revue)
        {
            model.RevuesDisponibles = _unitOfWork.Revues.Get(b => b.Active && b.RecherchePartielleDepuisGlobale).ToList();
            model.Revue = revue;
            model.GroupeIndex = GetGroupeIndex(revue);
        }

        public ActionResult Pdf(int id, string niceUrl)
        {
            var ar = _unitOfWork.ArticlesRevue.Get( b => b.Id == id, null, b => b.Revue).FirstOrDefault();

            if (ar == null)
            {
                throw new HttpException(404, "Not found");
            }

            MemoryStream stream = new MemoryStream();

            var fileName = String.Format("{0} ({1}).pdf", ar.Titre, ar.ShortTag);

            _revueService.CreatePdf(id, stream);

            stream.Flush();
            stream.Position = 0;

            return File(stream, "application/pdf");
        }

        public FileStreamResult Pdfs(int[] ids)
        {
            MemoryStream stream = new MemoryStream();

            var fileName = String.Format("plusieurs articles.pdf");

            _revueService.CreatePdf(ids, stream);

            stream.Flush(); //Always catches me out
            stream.Position = 0; //Not sure if this is required

            return File(stream, "application/pdf");
        }
    }
}
