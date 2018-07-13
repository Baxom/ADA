using ADA.Data.UnitOfWork;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Linq;

namespace ADA.Site.ApiControllers
{
    [RoutePrefix("api/fond")]
    public class FondController : ApiController
    {
        IUnitOfWork _unitOfWork = null;

        public FondController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        [Route("auto-complete/{idInformationFond}/{searchTerms}")]
        [Route("auto-complete/{idInformationFond}/{searchTerms}/{nb}")]
        public IHttpActionResult Autocomplete(int idInformationFond, string searchTerms, int nb = 10)
        {
            var resu = _unitOfWork.InformationFondMedia.Get(b => b.InformationFond.Id == idInformationFond && b.ValueString != null && b.ValueString.Contains(searchTerms)).Distinct().Take(nb).ToList();

            return Ok(resu);
        }

    }
}
