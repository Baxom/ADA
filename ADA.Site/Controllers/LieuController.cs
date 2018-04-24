using ADA.Data.UnitOfWork;
using ADA.Domain.Constantes;
using ADA.Infrastructure.PaginationHandler;
using ADA.Site.ActionResults;
using System;
using System.Web.Mvc;

namespace ADA.Site.ApiControllers
{
    public class LieuController : Controller
    {
        IUnitOfWork _unitOfWork;

        public LieuController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult AutoComplete(int? typeLieuId, int? typeLieuFonctionnelId, string filtre, int? nombreAutoComplete)
        {
            nombreAutoComplete = nombreAutoComplete ?? 5;

            TypeLieuFonctionnel? typeFonctionnel = null;

            if (typeLieuFonctionnelId.HasValue && Enum.IsDefined(typeof(TypeLieuFonctionnel), typeLieuFonctionnelId))
            {
                typeFonctionnel = (TypeLieuFonctionnel)typeLieuFonctionnelId.Value;
            }              
            

            return new JsonHttpStatusResult(200, _unitOfWork.Lieux.Paginate(new PaginationRequest(nombreAutoComplete.Value, 1),
                b => (!typeLieuId.HasValue || b.TypeLieu.Id == typeLieuId)
                    && (!typeFonctionnel.HasValue || b.TypeLieu.TypeFonctionnel == typeFonctionnel) 
                    && (filtre == null || b.Nom.Contains(filtre))).Data);
        }

        public ActionResult Get(int typeId)
        {
            return new JsonHttpStatusResult(200, _unitOfWork.Lieux.Get(b => b.TypeLieu.Id == typeId ));
        }

       
    }
}
