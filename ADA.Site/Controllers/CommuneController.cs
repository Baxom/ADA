using ADA.Data.UnitOfWork;
using ADA.Infrastructure.PaginationHandler;
using ADA.Site.ActionResults;
using System.Web.Mvc;

namespace ADA.Site.ApiControllers
{
    public class CommuneController : Controller
    {
        IUnitOfWork _unitOfWork;

        public CommuneController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ActionResult AutoComplete(string filtre, int? nombreAutoComplete)
        {
            nombreAutoComplete = nombreAutoComplete ?? 5;

            return new JsonHttpStatusResult(200, _unitOfWork.Lieux.Paginate(new PaginationRequest(nombreAutoComplete.Value, 1),
                b => filtre == null || b.Nom.Contains(filtre)).Data);
        }

    }
}
