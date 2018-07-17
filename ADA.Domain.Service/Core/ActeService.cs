using ADA.Data.UnitOfWork;
using ADA.Domain.Pretres;
using ADA.Domain.RegistresParoissiaux;
using ADA.Domain.Revues;
using ADA.Domain.Services.Interface;
using ADA.Infrastructure.Services.Interface.PdfManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Services.Core
{
    public class ActeService : IActeService
    {
        IUnitOfWork _uow;
        IPdfManager _pdfManager;

        string _fileMissingMessage = "Pensez à contacter l'administration des archives diocesaines d'Angers pour corriger ce problème";

        public ActeService(IUnitOfWork uow, IPdfManager pdfManager)
        {
            _uow = uow;
            _pdfManager = pdfManager;
        }

        public void CreatePdf(int acteId, System.IO.Stream memoryStream)
        {
            Acte acte = _uow.Actes.Get(b => b.Id == acteId, null, b => b.ParoisseRegistre).FirstOrDefault();
            var documentToMerge = acte.GetDocuments();

            var pdfManager = _pdfManager.Create(memoryStream);

            foreach (var doc in documentToMerge)
            {
                pdfManager.AddPdf(doc.NomCompletFichier, doc.Tag, _fileMissingMessage, acte.PagesReferences.ListePages.ToList());
            }

            pdfManager.Close();
        }

        public void CreatePdf(IEnumerable<int> acteIds, Stream memoryStream)
        {
            var actes = _uow.Actes.Get(b => acteIds.Contains(b.Id),
                p => p.OrderBy(b => b.ParoisseRegistre.Nom).ThenBy(b => b.AnneeRegistreParoissial).ThenBy(b => b.Pages),
                b => b.ParoisseRegistre);

            var pdfManager = _pdfManager.Create(memoryStream);
            PdfTableOfContent toc = new PdfTableOfContent("Sommaire");
            var totalPage = 0;

            foreach (var acte in actes)
            {

                toc.AddContent(String.Format("{0} ({1})", acte.ShortTag, acte.ShortTag), totalPage + 1);

                var documentToMerge = acte.GetDocuments();

                foreach (var doc in documentToMerge)
                {
                    totalPage += pdfManager.AddPdf(doc.NomCompletFichier, doc.Tag, _fileMissingMessage, acte.PagesReferences.ListePages.ToList());
                }

            }

            toc.DecalePage(pdfManager.ComputeTableOfContent(toc));
            pdfManager.AddTableOfContent(toc);
            pdfManager.Close();

        }
    }
}
