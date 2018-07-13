using ADA.Data.UnitOfWork;
using ADA.Domain.Catalogues;
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
    public class CatalogueService : ICatalogueService
    {
        IUnitOfWork _uow;
        IPdfManager _pdfManager;

        string _fileMissingMessage = "Pensez à contacter l'administration des archives diocesaines d'Angers pour corriger ce problème";

        public CatalogueService(IUnitOfWork uow, IPdfManager pdfManager)
        {
            _uow = uow;
            _pdfManager = pdfManager;
        }

        public void CreatePdf(int catalogueId, string searchTerms, System.IO.Stream memoryStream)
        {
            Catalogue catalogue = _uow.Catalogues.Get(b => b.Id == catalogueId, null, b => b.SousSerie.Serie).FirstOrDefault();
            string motRecherche = searchTerms;
            var pdfManager = _pdfManager.Create(memoryStream);

            pdfManager.AddNewPage();
            if(!string.IsNullOrWhiteSpace(motRecherche)) pdfManager.WriteText(String.Format("[Mot recherche : {0}]", motRecherche), null, null, null, true);

            var serie = catalogue.SousSerie.Serie;
            var serieName = String.Format("Série {0} - {1}", serie.Code, serie.Nom);

            pdfManager.WriteTitle(serieName, 0, true);
            pdfManager.WriteTitle(catalogue.SousSerie.Nom, 1, true, true, false, true);
                
            pdfManager.WriteCatalogue(catalogue.Titre, catalogue.Cote, motRecherche);
            pdfManager.Close();
        }

        public void CreatePdf(IEnumerable<int> catalogueIds, string searchTerms, Stream memoryStream)
        {
            List<Catalogue> catalogues = _uow.Catalogues.Get(b => catalogueIds.Contains(b.Id), null, b => b.SousSerie.Serie).ToList();
            string motRecherche = searchTerms;
            var pdfManager = _pdfManager.Create(memoryStream);
            int countGroupSerie = 0;
            int countGroupSousSerie = 0;
            pdfManager.AddNewPage();
            if (!string.IsNullOrWhiteSpace(motRecherche)) pdfManager.WriteText(String.Format("[Mot recherche : {0}]", motRecherche), null, null, null, true);

            foreach(var groupeSerie in catalogues.GroupBy( grp => grp.SousSerie.Serie ) )
            {

                if(countGroupSerie++ > 0)
                {
                    pdfManager.WriteText(" ");
                    pdfManager.WriteText(" ");
                }
                
                var serieName = String.Format("Série {0} - {1}", groupeSerie.Key.Code, groupeSerie.Key.Nom);
                pdfManager.WriteTitle(serieName, 0, true);
                countGroupSousSerie = 0;
                foreach (var groupeSousSerie in groupeSerie.GroupBy(grp => grp.SousSerie))
                {
                    if (countGroupSousSerie++ > 0)
                    {
                        pdfManager.WriteText(" ");
                        pdfManager.WriteText(" ");
                    }

                    var sousSerieName = groupeSousSerie.Key.Nom;
                    pdfManager.WriteTitle(sousSerieName, 1, true, true, false, true);
                    int countCatalogue = 0;
                    foreach (var catalogue in groupeSousSerie.OrderBy(c => c.Cote))
                    {
                        if (countCatalogue++ > 0)
                        {
                            pdfManager.WriteText(" ");
                        }
                        pdfManager.WriteCatalogue(catalogue.Titre, catalogue.Cote, motRecherche); 
                    }
                }

                
            }

            pdfManager.Close();

        }
    }
}
