using ADA.Data.UnitOfWork;
using ADA.Infrastructure.Services.Interface.PdfManager;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ADA.Site.Controllers
{
    public class HomeController : Controller
    {

        public IUnitOfWork _unitOfWork { get; set; }
        IPdfManager _pdfManager;

        public HomeController(ILogger logger, IUnitOfWork unitOfWork, IPdfManager pdfManager) 
        {
            _unitOfWork = unitOfWork;
            _pdfManager = pdfManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Contact()
        {

            //var pretres = _unitOfWork.Pretres.GetPretreByLieu(null, null, null, null, null, null);

            //string[] allfiles = System.IO.Directory.GetFiles(@"C:\Users\Benoit\Documents\Visual Studio 2013\Projects\ADA\ADA.Site\archives\revues", "*.pdf", System.IO.SearchOption.AllDirectories);

            //FileStream fs = new FileStream(@"c:\test\tous.pdf", FileMode.Create, FileAccess.Write);
            //var pdfm = _pdfManager.Create(fs);
            //PdfTableOfContent toc = new PdfTableOfContent("");

            //var totalPage = 0;
            //allfiles.ToList().ForEach(b => {

            //    toc.AddContent(Path.GetFileName(b), totalPage + 1);

            //    totalPage += pdfm.AddPdf(b);
            //});

            //var decalage = pdfm.ComputeTableOfContent(toc);
            //toc.DecalePage(decalage);

            //pdfm.AddTableOfContent(toc);

            //pdfm.Close();
            //fs.Close();

            return View();
        }
    }
}