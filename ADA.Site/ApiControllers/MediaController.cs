using ADA.Data.UnitOfWork;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ADA.Site.ApiControllers
{
    [RoutePrefix("api/media")]
    public class MediaController : ApiController
    {
        IUnitOfWork _unitOfWork = null;

        public MediaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        [Route("stream/{id}")]
        public IHttpActionResult Stream(int id)
        {

            var medium = _unitOfWork.Media.FindBy(id);

            if(medium == null)
            {
                return NotFound();
            }

            if(medium.Type != Domain.Constantes.TypeMedium.Video && medium.Type != Domain.Constantes.TypeMedium.Son)
            {
                return InternalServerError();
            }

            var root = System.Web.HttpContext.Current.Server.MapPath("~/");
                
            string path = Path.Combine(root, medium.RepertoireNom.TrimStart('/', '\\').Replace("\\", " /"));
            if (!File.Exists(path)) return NotFound();

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);


            if (Request.Headers.Range != null)
            {
                // Return part of the video
                HttpResponseMessage partialResponse = Request.CreateResponse(HttpStatusCode.PartialContent);
                partialResponse.Content = new ByteRangeStreamContent(fs, Request.Headers.Range, System.Web.MimeMapping.GetMimeMapping(medium.FileName));
                return ResponseMessage(partialResponse);
            }
            else
            {
                // Return complete video
                HttpResponseMessage fullResponse = Request.CreateResponse(HttpStatusCode.OK);
                fullResponse.Content = new StreamContent(fs);
                fullResponse.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = medium.FileName
                };
                fullResponse.Content.Headers.ContentType = fullResponse.Content.Headers.ContentType ?? new MediaTypeHeaderValue(System.Web.MimeMapping.GetMimeMapping(medium.FileName));
                fullResponse.Content.Headers.ContentType.MediaType = System.Web.MimeMapping.GetMimeMapping(medium.FileName);
                
                return ResponseMessage(fullResponse);
            }


        }

        [HttpGet]
        [Route("content/{id}")]
        public IHttpActionResult Content(int id)
        {
            return GetContent(id, "attachment");

        }

        private IHttpActionResult GetContent(int id, string contentDisposition)
        {
            var medium = _unitOfWork.Media.FindBy(id);

            if (medium == null)
            {
                return NotFound();
            }

            var root = System.Web.HttpContext.Current.Server.MapPath("~/");

            string path = Path.Combine(root, medium.RepertoireNom.TrimStart('/', '\\').Replace("\\", " /"));

            if (!File.Exists(path)) return NotFound();

            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);

            HttpResponseMessage fullResponse = Request.CreateResponse(HttpStatusCode.OK);
            fullResponse.Content = new StreamContent(fs);
            fullResponse.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue(contentDisposition)
            {
                FileName = medium.FileName
            };
            fullResponse.Content.Headers.ContentType = fullResponse.Content.Headers.ContentType ?? new MediaTypeHeaderValue(System.Web.MimeMapping.GetMimeMapping(medium.FileName));
            fullResponse.Content.Headers.ContentType.MediaType = System.Web.MimeMapping.GetMimeMapping(medium.FileName);

            return ResponseMessage(fullResponse);
        }

        [HttpGet]
        [Route("inline-content/{id}/{niceUrl}")]
        public IHttpActionResult InlineContent(int id, string niceUrl)
        {
            return GetContent(id, "inline");
        }

    }
}
