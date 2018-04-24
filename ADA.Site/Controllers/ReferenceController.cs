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
    public class ReferenceController : Controller
    {
        IUnitOfWork _unitOfWork;

        public ReferenceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult Commune(string filtre)
        {
            var communes = _unitOfWork.Communes.Paginate(new PaginationRequest(10, 1), b => b.CodePostal.Contains(filtre) || b.Nom.Contains(filtre));

            return new JsonHttpStatusResult(200, communes);
        }
    }
}
