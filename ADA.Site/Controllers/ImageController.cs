using ADA.Site.ActionResults;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ADA.Site.Controllers
{
    public class ImageController : Controller
    {
        [OutputCache(Duration = 3600, VaryByParam = "filename")]
        public ActionResult Thumbnail(string filename)
        {

            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Path.Combine(System.Configuration.ConfigurationManager.AppSettings["RepertoirePhotoPretre"], filename).TrimStart('/', '\\').Replace("\\", "/"));
            WebImage img = null;


            if (!System.IO.File.Exists(path)) return HttpNotFound();

            img = new WebImage(path);
            img.Resize(100, 100, true, true);
            return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
        }

        [OutputCache(Duration = 3600, VaryByParam = "filename")]
        public ActionResult Image(string filename)
        {

            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Path.Combine(System.Configuration.ConfigurationManager.AppSettings["RepertoirePhotoPretre"], filename).TrimStart('/', '\\').Replace("\\", "/"));
            WebImage img = null;


            if (!System.IO.File.Exists(path)) return HttpNotFound();

            img = new WebImage(path);
           // img.Resize(100, 100, true, true);
            return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
        }
    }
}