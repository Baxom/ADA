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
using ADA.Infrastructure.Services.Interface.WordSearchParser;

namespace ADA.Site.Controllers
{
    public class MediaController : ImageController
    {
        IUnitOfWork _unitOfWork;
        IWordSearchParser _wordSearchparser;

        public MediaController(IUnitOfWork unitOfWork, IWordSearchParser wordSearchparser)
        {
            _unitOfWork = unitOfWork;
            _wordSearchparser = wordSearchparser;
        }

        // GET: MEdia
        public ActionResult Index()
        {
            return View("~/Views/Media/Recherche.cshtml", new RechercheMediaViewModel(false));
        }

       
        public ActionResult Recherche(RechercheMediaViewModel model)
        {

            var titre = _unitOfWork.FTSContains(model.Nom);

            Expression<Func<Medium, bool>> filter = b => (model.Nom == null || b.Titre.Contains(titre) || b.Tags.Any( t => t.Tag.Libelle.Contains(model.Nom))) && (b.Type != Domain.Constantes.TypeMedium.NonMedia) && (!model.TypeMedium.HasValue || model.TypeMedium.Value == b.Type );
            model.Resultats = _unitOfWork.Media
                .Paginate(new PaginationRequest(model.Pagination.Valeur, model.Page), filter,
                 b => b.OrderByDescending( o => o.Titre.Contains(model.Nom) ),
                b => b.Tags.Select( t => t.Tag )).ToPagedListMvc(model.Page, model.Pagination.Valeur);

            return View("~/Views/Media/Recherche.cshtml", model);
        }

        [OutputCache(Duration = 3600, VaryByParam = "id,width,height")]
        public ActionResult Thumbnail(int id, int width = 100, int height = 100)
        {
            var medium = _unitOfWork.Media.FindBy(id);

            if (medium == null)
            {
                throw new HttpException(404, "Not found");
            }

            var imagePath = medium.RepertoireNomThumbnail;

            // si pas de miniature et que c est une image, alors on affiche l'image
            if(String.IsNullOrEmpty(imagePath) && medium.Type == Domain.Constantes.TypeMedium.Photo)
            {
                imagePath = medium.RepertoireNom;
            }

            return Thumbnail(imagePath, width, height);
        }

        [OutputCache(Duration = 3600, VaryByParam = "id")]
        public ActionResult Image(int id)
        {
            var medium = _unitOfWork.Media.FindBy(id);

            if (medium == null)
            {
                throw new HttpException(404, "Not found");
            }
            
            return Image(medium.RepertoireNom);
        }

    }
}
