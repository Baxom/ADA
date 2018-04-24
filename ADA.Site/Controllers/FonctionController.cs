using ADA.Data.UnitOfWork;
using ADA.Domain.Pretres;
using ADA.Infrastructure.PaginationHandler;
using ADA.Site.ActionResults;
using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using ADA.Domain.Fonctions;

namespace ADA.Site.ApiControllers
{
    public class FonctionController : Controller
    {
        IUnitOfWork _unitOfWork;

        public FonctionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Get(int? typeLieuId, int? lieuId)
        {
            List<Fonction> fonctions = new List<Fonction>();


            if(typeLieuId.HasValue)
                fonctions.AddRange(_unitOfWork.FonctionsTypeLieu.Get(b => b.TypeLieu.Id == typeLieuId.Value, null, b => b.Fonction).Select(b => b.Fonction));

            if (lieuId.HasValue)
                fonctions.AddRange(_unitOfWork.FonctionsLieu.Get(b => b.Lieu.Id == lieuId.Value, null, b => b.Fonction).Select(b => b.Fonction));

            fonctions = fonctions.OrderBy( b => b.Nom).ToList();

            return new JsonHttpStatusResult(200, fonctions);
        }
    }
}
