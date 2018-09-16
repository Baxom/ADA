using ADA.Data.UnitOfWork;
using ADA.Domain.Pretres;
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
    public class ArticleRevueService : IArticleRevueService
    {
        IUnitOfWork _uow;
        IPdfManager _pdfManager;

        string _fileMissingMessage = "Pensez à contacter l'administration des archives diocesaines d'Angers pour corriger ce problème";

        public ArticleRevueService(IUnitOfWork uow, IPdfManager pdfManager)
        {
            _uow = uow;
            _pdfManager = pdfManager;
        }

        public void CreatePdf(int articleRevueId, System.IO.Stream memoryStream)
        {
            ArticleRevue articleRevue = _uow.ArticlesRevue.Get(b => b.Id == articleRevueId, null, b => b.Revue.RevueMere).FirstOrDefault();
            var documentToMerge = articleRevue.GetDocuments();

            var pdfManager = _pdfManager.Create(memoryStream);

            foreach (var doc in documentToMerge)
            {
                pdfManager.AddPdf(doc.NomCompletFichier, doc.Tag, _fileMissingMessage, articleRevue.PagesReferences.ListePages.ToList(), true);
            }

            pdfManager.Close();
        }

        public void CreatePdf(IEnumerable<int> articleRevueIds, Stream memoryStream)
        {
            var articlesRevues = _uow.ArticlesRevue.Get(b => articleRevueIds.Contains(b.Id), 
                p => p.OrderBy(b => b.Revue.Nom).ThenBy(b => b.PeriodePublication).ThenBy( b => b.Pages), 
                b => b.Revue.RevueMere);

            var pdfManager = _pdfManager.Create(memoryStream);
            PdfTableOfContent toc = new PdfTableOfContent("Sommaire");
            var totalPage = 0;

            foreach (var articleRevue in articlesRevues)
            {

                toc.AddContent( String.Format("{0} ({1})", articleRevue.Titre, articleRevue.ShortTag), totalPage + 1);

                var documentToMerge = articleRevue.GetDocuments();
                foreach (var doc in documentToMerge)
                {
                    totalPage += pdfManager.AddPdf(doc.NomCompletFichier, doc.Tag, _fileMissingMessage, articleRevue.PagesReferences.ListePages.ToList(), true);
                  
                }

            }
            
            toc.DecalePage(pdfManager.ComputeTableOfContent(toc));
            pdfManager.AddTableOfContent(toc);
            pdfManager.Close();
            
        }
    }
}
