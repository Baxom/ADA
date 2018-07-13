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
using ADA.Domain.Media;
using ADA.Domain.Fonds;

namespace ADA.Site.Controllers
{
    public class FondController : Controller
    {
        IUnitOfWork _unitOfWork;

        public FondController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Fond
        public ActionResult Index(RechercheFondViewModel model)
        {
            model.Recherche = false;
            var fond = GetFond(model.Id);

            if (fond == null && !model.Id.HasValue)
            {
                throw new HttpException(404, "Not found");
            }

            BuildModel(model, fond);

            return View("~/Views/Fonds/Recherche.cshtml", model);
        }

        public ActionResult Recherche(RechercheFondViewModel model)
        {
            var fond = GetFond(model.Id);

            if (fond == null && !model.Id.HasValue)
            {
                throw new HttpException(404, "Not found");
            }
            BuildModel(model, fond);
            var ids = _unitOfWork.FondMediums.GetFondMediumIds(model.Id.Value,
                model.Nom,
                model.SelectedIndex.Where( b => b.Value.HasValue).Select( b => b.Value.Value),
                model.InformationsFilters,
                new PaginationRequest(model.Pagination.Valeur, model.Page));

            Expression<Func<Medium, bool>> filter = b => ids.Data.Contains(b.Id);
            model.Resultats = _unitOfWork.FondMediums.Get(b => ids.Data.Contains(b.Id),
                b => b.OrderByDescending(o => o.Titre.Contains(model.Nom)), b => b.ColonneFondMedium, b => b.Tags.Select( t => t.Tag))
                .ToPagedListMvc(model.Page, model.Pagination.Valeur, ids.CountResult);
            
            return View("~/Views/Fonds/Recherche.cshtml", model);
        }

        public ActionResult Description(RechercheFondViewModel model)
        {
            var fond = GetFond(model.Id);

            if (fond == null && !model.Id.HasValue)
            {
                throw new HttpException(404, "Not found");
            }


            BuildModel(model, fond);

            return View("~/Views/Fonds/Description.cshtml", model);
        }


        private void BuildModel(RechercheFondViewModel model, Fond fond)
        {
            model.Fond = fond;
            model.GroupeIndex = GetGroupeIndex(fond);
        }

        private IEnumerable<GroupeIndexViewModel> GetGroupeIndex(ADA.Domain.Fonds.Fond fond)
        {

            List<GroupeIndexViewModel> giv = new List<GroupeIndexViewModel>();

            if (fond == null) return giv;

            fond.FondGroupeIndex.Select(b => b.GroupeIndex).ToList().ForEach(b => {

                var groupe = new GroupeIndexViewModel();
                groupe.Id = b.Id;
                groupe.Libelle = b.Libelle;

                List<IndexViewModel> index = new List<IndexViewModel>();

                index.Add(new IndexViewModel() { Id = null, Libelle = "Tous" });
                index.AddRange(b.Index.Select(i => new IndexViewModel() { Id = i.Id, Libelle = i.Libelle }));
                groupe.Index = index;

                giv.Add(groupe);
            });

            return giv;
        }


        private Domain.Fonds.Fond GetFond(int? id)
        {
            var revue = _unitOfWork.Fonds.Get(b => b.Id == id,
                null,
                b => b.FondGroupeIndex.Select(rgi => rgi.GroupeIndex.Index),
                b => b.InformationRechercheFonds.Select( rech => rech.InformationFiltreInjectionFonds ),
                b => b.InformationAffichageFonds,
                b => b.InformationFonds,
                b => b.FondsFils).FirstOrDefault(); 

            return revue;
        }

    }
}
