using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Infrastructure.Services.Interface.PdfManager
{
    public interface IPdfManager
    {
        IPdfManager Create(Stream stream);
        int AddPdf(string pdfPath, string stampString = null, string fileNotFoundMessage = null, List<int> pageIndexToPrint = null);
        int AddImage(string imagePath, string titre = null, string stampString = null, string fileNotFoundMessage = null);
        int AddTableOfContent(PdfTableOfContent toc);
        int ComputeTableOfContent(PdfTableOfContent toc);
        int GetCurrentPageNumber();
        void WriteTitle(string text, int level = 0, bool center = false, bool underline = false, bool bold = false, bool italic = false);
        void WriteText(string text, string higlightedText = null, Color? higlightedTextColor = null, Color? backgroundHighlightedTextColor = null, bool bold = false);
        void WriteCatalogue(string text, string cote, string searchText);
        void AddNewPage();
        void Close();
    }

    public class PdfTableOfContent {

        public PdfTableOfContent(string titreDocument)
	    {
            TitreDocument = titreDocument;
            _contents = new List<ItemTableOfContent>();
	    }

        List<ItemTableOfContent> _contents;

        public string TitreDocument { get; set; }
        internal IEnumerable<ItemTableOfContent> Contents { 
            get{
                return _contents;
            }
        }

        public void AddContent(string titre, int page)
        {
            _contents.Add(new ItemTableOfContent(titre, page));
            _contents = _contents.OrderBy(b => b.RealPage).ToList();
        }

        public void DecalePage(int nbPage)
        {
            _contents.ForEach(b => b.PageNumber += nbPage);
        }
        
    }

    internal class ItemTableOfContent
    {
        public ItemTableOfContent(string titre, int page)
        {
            Titre = titre;
            PageNumber = page;
            RealPage = page;
        }

        public string AnchorName { 
            get {
                return "p" + RealPage.ToString();
            } 
        }

        public string Titre { get; set; }
        public int RealPage { get; set; }
        public int PageNumber { get; set; }
    }



}
