using ADA.Data.UnitOfWork;
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
        protected ActionResult Thumbnail(string filename, int width = 100, int height = 100)
        {
            var pathThumbnail = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/TempThumbnail"), Path.GetDirectoryName(filename));
            WebImage img = null;
            if (!Directory.Exists(pathThumbnail)) Directory.CreateDirectory(pathThumbnail);

            filename = filename.TrimStart('/', '\\').Replace("\\", "/");
            var thumbnailSize = String.Format("{0}x{0}", width, height);
            var filenameThumbnailPath = Path.Combine(pathThumbnail, String.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(filename), thumbnailSize, Path.GetExtension(filename)));

            if(System.IO.File.Exists(filenameThumbnailPath))
            {
                img = new WebImage(filenameThumbnailPath);
                return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
            }
            else
            {
                var path = System.Web.Hosting.HostingEnvironment.MapPath("~/" + filename.TrimStart('/', '\\').Replace("\\", "/"));

                if (!System.IO.File.Exists(path)) return HttpNotFound();

                img = new WebImage(path);
                img.Resize(width, height, true, true);

                img.Save(filenameThumbnailPath);

                return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
            }
           
        }

        protected ActionResult Image(string filename)
        {

            // var path = System.Web.Hosting.HostingEnvironment.MapPath("~/" + Path.Combine(System.Configuration.ConfigurationManager.AppSettings["RepertoirePhotoPretre"], filename).TrimStart('/', '\\').Replace("\\", "/"));
            var path = System.Web.Hosting.HostingEnvironment.MapPath("~/" + filename.TrimStart('/', '\\').Replace("\\", "/"));
            WebImage img = null;
            
            if (!System.IO.File.Exists(path)) return HttpNotFound();

            img = new WebImage(path);
            return new ImageResult(new MemoryStream(img.GetBytes()), "binary/octet-stream");
        }
    }
}
