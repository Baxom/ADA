using ADA.Data.UnitOfWork;
using ADA.Domain.Pretres;
using ADA.Domain.Services.Interface;
using ADA.Infrastructure.Services.Interface.PdfManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ADA.Infrastructure.Extentions;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Domain.Services.Core
{
    public class PretreService : IPretreService
    {
        IUnitOfWork _uow;
        IPdfManager _pdfManager;

        string _fileMissingMessage = "Pensez à contacter l'administration des archives diocesaines d'Angers pour corriger ce problème";

        public PretreService(IUnitOfWork uow, IPdfManager pdfManager)
        {
            _uow = uow;
            _pdfManager = pdfManager;
        }

        public void CreatePdf(int pretreId, System.IO.Stream memoryStream)
        {
            Pretre pretre = _uow.Pretres.Get(b => b.Id == pretreId, null, 
                b => b.Documents, 
                b => b.Photos, 
                b => b.ArticlesRevue.Select(ar => ar.Revue),
                b => b.FonctionsLieu.Select( fl => fl.Fonction ),
                b => b.FonctionsLieu.Select( fl => fl.Lieu.TypeLieu )).FirstOrDefault();
            
            var pdfManager = _pdfManager.Create(memoryStream);

            WritePretreToPdf(pdfManager, pretre);

            pdfManager.Close();
        }

        public void CreatePdf(IEnumerable<int> pretreIds, Stream memoryStream)
        {
            var pretres = _uow.Pretres.Get(b => pretreIds.Contains(b.Id), null, 
                b => b.Documents, 
                b => b.Photos, 
                b => b.ArticlesRevue.Select(ar => ar.Revue),
                b => b.FonctionsLieu.Select(fl => fl.Fonction),
                b => b.FonctionsLieu.Select(fl => fl.Lieu.TypeLieu)).OrderByExtList<Pretre, int>(pretreIds.ToList(), b => b.Id);
            
            var pdfManager = _pdfManager.Create(memoryStream);
            PdfTableOfContent toc = new PdfTableOfContent("Sommaire");
            var totalPage = 0;

            foreach (var pretre in pretres)
            {
                toc.AddContent(pretre.NomComplet, totalPage + 1);
                totalPage += WritePretreToPdf(pdfManager, pretre);
            }
            
            toc.DecalePage(pdfManager.ComputeTableOfContent(toc));
            pdfManager.AddTableOfContent(toc);
            pdfManager.Close();
            
        }

        private int WritePretreToPdf(IPdfManager pdfManager, Pretre pretre)
        {
            int totalPage = 0;

            if (pretre.Photos.Any())
            {
                pdfManager.AddImage(pretre.Photos.First().NomCompletFichier,pretre.NomEtDateVie, null, _fileMissingMessage);
                totalPage++;
            }

            
            var pdfFilesToMerge = pretre.Documents;
           
            foreach (var pdf in pdfFilesToMerge)
            {
                var nbPagePdf = pdfManager.AddPdf(pdf.NomCompletFichier, null, _fileMissingMessage, null, true);
                totalPage += nbPagePdf;
            }

            foreach(var articleRevue in pretre.ArticlesRevue)
            {
                var documentToMerge = articleRevue.GetDocuments();
                foreach (var doc in documentToMerge)
                {
                    totalPage += pdfManager.AddPdf(doc.NomCompletFichier, doc.Tag, _fileMissingMessage, articleRevue.PagesReferences.ListePages.ToList());
                }

            }

            var currentNumberOfPage = pdfManager.GetCurrentPageNumber();

            pdfManager.AddNewPage();
            pdfManager.WriteTitle("Fonctions " + pretre.NomEtDateVie, 1, true, true, false, true);
            if (pretre.FonctionsLieu.Any())
            {
                foreach (var fl in pretre.GetFormatedFonctions())
                {
                    pdfManager.WriteText(fl);
                }
            }
            else
            {
                pdfManager.WriteText("Aucune fonctions pour ce prêtre.");
            }

            totalPage = totalPage - currentNumberOfPage + pdfManager.GetCurrentPageNumber();

            return totalPage;
        }
    }
}
