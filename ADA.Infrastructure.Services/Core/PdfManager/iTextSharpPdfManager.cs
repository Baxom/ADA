using ADA.Infrastructure.Services.Interface.PdfManager;
using iTextSharp.awt.geom;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ADA.Infrastructure.Services.Core.PdfManager
{
    public class iTextSharpPdfManager : IPdfManager, IDisposable
    {
        Rectangle pageSize = PageSize.A4;
        bool _badInitialisation = true;
        Stream _outputContent = null;

        MemoryStream _streamContent = null;
        MemoryStream _streamToc = null;
        Document _docContent = null;
        Document _docToc = null;
        PdfWriter _writerContent = null;
        PdfWriter _writerToc = null;
        ParagraphBorder _border = new ParagraphBorder();

        PdfTableOfContent _toc;
        
        List<PdfReader> _readers = null;

        Font fontTitrePrincipal = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false), 16);
        Font fontTitreSecondaire = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false), 13);
        Font fontText = new Font(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), 12);
        Font fontStamp = new Font(BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, false), 10);

        public iTextSharpPdfManager()
        {

        }

        private iTextSharpPdfManager(Stream stream)
        {
            _badInitialisation = false;
            _outputContent = stream;
            InitDocuments();
            _readers = new List<PdfReader>();

        }

        private void InitDocuments()
        {
            _streamContent = new MemoryStream();
            //_docContent = new Document(pageSize);
            _docContent = new Document();
            _writerContent = PdfWriter.GetInstance(_docContent, _streamContent);
            _writerContent.PageEvent = _border;
            _docContent.Open();

            _streamToc = new MemoryStream();
           // _docToc = new Document(pageSize);
            _docToc = new Document();
            _writerToc = PdfWriter.GetInstance(_docToc, _streamToc);
            _docToc.Open();
        }

        /// <summary>
        /// Factory
        /// </summary>
        /// <returns></returns>
        public IPdfManager Create(Stream stream)
        {
            return new iTextSharpPdfManager(stream);
        }

        public int AddPdf(string pdfPath, string stampString = null, string fileNotFoundMessage = null, List<int> pageIndexToPrint = null)
        {

            pageIndexToPrint = pageIndexToPrint ?? new List<int>();
            pageIndexToPrint = pageIndexToPrint.OrderBy(b => b).ToList();

           

            if (!File.Exists(pdfPath))
            {
                AddNewContentPage();
                CreateMissingFilePage("Pdf introuvable", pdfPath, fileNotFoundMessage);
                return 1;
            }

            var reader = new PdfReader(pdfPath);
            _readers.Add(reader);

            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                if (pageIndexToPrint.Any() && !pageIndexToPrint.Contains(i)) continue;

                AddNewContentPage(reader.GetPageSizeWithRotation(i));
                
                PdfImportedPage page = _writerContent.GetImportedPage(reader, i);
               
                // AddNewContentPage(page);

             //   _writerContent.DirectContent.AddTemplate(page, 0, 0);
                _writerContent.DirectContent.AddTemplate(page, CreateTransform(page, page.Rotation));
                //_writerContent.DirectContent.Add(page);
                CreateStamp(stampString);
            }

            return pageIndexToPrint.Any() ? pageIndexToPrint.Count : reader.NumberOfPages;
        }

        private void CreateStamp(string stampString)
        {
            if (!String.IsNullOrEmpty(stampString))
            {
                ColumnText ct = new ColumnText(_writerContent.DirectContent);
                ct.SetSimpleColumn(_docContent.Left, _docContent.Top + _docContent.TopMargin - 20, _docContent.Right, _docContent.Top + _docContent.TopMargin - 10, 0f, Element.ALIGN_CENTER);
                ct.AddText(new Phrase(stampString, fontStamp));
                ct.Go();
            }

        }

        private AffineTransform CreateTransform(PdfImportedPage page, int rotation)
        {
           
            var docWidth = _docContent.Right - _docContent.Left;
            var docHeight = _docContent.Top - _docContent.Bottom;

            AffineTransform rotate = AffineTransform.GetRotateInstance((Math.PI * (360 - rotation)) / 180, page.Width / 2, page.Height / 2);
            AffineTransform scale = AffineTransform.GetScaleInstance(docWidth / page.Width, docHeight / page.Height);
            AffineTransform translateMargin = AffineTransform.GetTranslateInstance(_docContent.LeftMargin, _docContent.BottomMargin);
            if(rotation == 90)
            {
                scale = AffineTransform.GetScaleInstance(docWidth / page.Height, docHeight / page.Width);
                translateMargin = AffineTransform.GetTranslateInstance(175, -175);
            }

            
            AffineTransform transform = AffineTransform.GetTranslateInstance(0, 0);

            transform.preConcatenate(rotate);
            transform.preConcatenate(scale);
            transform.preConcatenate(translateMargin);
            return transform;
        }

        public void Close()
        {
            AssertGoodInitialization();
            _docContent.Close();
            if(_toc != null) _docToc.Close();
            
            // concatenation toc et content
            ConcatTocAndContentToOutput();

        }

        private void ConcatTocAndContentToOutput()
        {
            Document document = new Document(PageSize.A4);
            PdfCopy copyWriter = new PdfCopy(document, _outputContent);
            document.Open();
            copyWriter.CloseStream = false;

            if (_toc != null)
            {
                var readerToc = new PdfReader((byte[])_streamToc.ToArray());
                readerToc.ConsolidateNamedDestinations();
                _readers.Add(readerToc);
                for (int i = 1; i <= readerToc.NumberOfPages; i++)
                {
                    PdfImportedPage page = copyWriter.GetImportedPage(readerToc, i);
                    copyWriter.AddPage(page);
                }
            }
            
            var readerContent = new PdfReader((byte[])_streamContent.ToArray());
            _readers.Add(readerContent);
            for (int i = 1; i <= readerContent.NumberOfPages; i++)
            {
                PdfImportedPage page = copyWriter.GetImportedPage(readerContent, i);
                var stamp = copyWriter.CreatePageStamp(page);

                if (_toc != null)
                {
                    var item = _toc.Contents.Where(b => b.RealPage == i).FirstOrDefault();
                    if (item != null)
                    {
                        CreateAnchor(stamp, item);
                    }
                }
                copyWriter.AddPage(page);
            }

            copyWriter.Close();
        }

        private static void CreateAnchor(PdfCopy.PageStamp stamp, ItemTableOfContent item)
        {
            var chunk = new Chunk(new String(new char[] { '\u2007' }));
            chunk.SetLocalDestination(item.AnchorName);
            ColumnText ct = new ColumnText(stamp.GetOverContent());
            ct.SetSimpleColumn(36, 700, 559, 750);
            ct.AddText(chunk);
            ct.Go();
        }

        private void AssertGoodInitialization()
        {
            if (_badInitialisation) throw new Exception("Le manager doit être instancié à l'aide de la méthode Create()");
        }

        public int ComputeTableOfContent(PdfTableOfContent toc)
        {
            AssertGoodInitialization();
            var numberOfPage = 0;

            using (MemoryStream stream = new MemoryStream())
            {

                var doc = new Document(pageSize);
                var writer = PdfWriter.GetInstance(doc, stream);
                doc.Open();

                numberOfPage = CreateTableOfContent(toc, writer, doc);
            }

            return numberOfPage;

        }

        public void Dispose()
        {   
            if (_streamContent != null) _streamContent.Dispose();
        }

        public int AddTableOfContent(PdfTableOfContent toc)
        {
            AssertGoodInitialization();
            _toc = toc;

            return CreateTableOfContent(_toc, _writerToc, _docToc);
        }

        private int CreateTableOfContent(PdfTableOfContent toc, PdfWriter writer, Document doc)
        {   
            _docToc.NewPage();

            Paragraph p;
            PdfAction action;
            PdfAnnotation link;

            if (!String.IsNullOrEmpty(toc.TitreDocument))
            {
                p = new Paragraph(toc.TitreDocument, fontTitrePrincipal);
                p.Alignment = Element.ALIGN_CENTER;
                doc.Add(p);
                // Un espace ? peut mieux faire peut etre...
                doc.Add(new Paragraph(" "));
            }

            foreach (var content in toc.Contents)
            {
                action = PdfAction.GotoLocalPage(content.AnchorName, false);
                p = new Paragraph(content.Titre, fontText);
                p.Add(new Chunk(new DottedLineSeparator()));
                p.Add(new Chunk(content.PageNumber.ToString(), fontText));

                float topY = writer.GetVerticalPosition(false);
                
                doc.Add(p);

                float bottomYY = writer.GetVerticalPosition(false);

                link = new PdfAnnotation(writer, doc.Left, bottomYY, doc.Right, topY, action);
                writer.AddAnnotation(link);
            }

            var numberOfPage = writer.PageNumber;

            return numberOfPage;
        }

        public int AddImage(string imagePath, string titre = null, string stampString = null, string fileNotFoundMessage = null)
        {
            AssertGoodInitialization();
            
            if (!File.Exists(imagePath))
            {
                CreateMissingFilePage("Photo introuvable", imagePath, fileNotFoundMessage);
            }
            else  {

                AddNewContentPage();
                CreateStamp(stampString);
                WriteText("\n", null, null, null, false);
                WriteTitle(titre, 1, true, true, false, true);
                iTextSharp.text.Image pic = iTextSharp.text.Image.GetInstance(imagePath);

                var docHeight = PageSize.A4.Height - _docContent.BottomMargin - _docContent.TopMargin - 150;
                var docWidth = PageSize.A4.Width - _docContent.LeftMargin - _docContent.RightMargin - 150;
                if (pic.Width > docWidth || pic.Height > docHeight)
                {
                    var scalePercent = docWidth / pic.Width * 100;
                    scalePercent = Math.Min(scalePercent, docHeight / pic.Height * 100);

                    pic.ScalePercent(scalePercent);
                }
                // centrage de l'image
                pic.SetAbsolutePosition((PageSize.A4.Width - pic.ScaledWidth) / 2, (PageSize.A4.Height - pic.ScaledHeight) / 2);
                pic.Border = iTextSharp.text.Rectangle.BOX;
                pic.BorderColor = iTextSharp.text.BaseColor.BLACK;
                pic.BorderWidth = 1f;
                _docContent.Add(pic);
            }
            
            return 1;
        }

        private void CreateMissingFilePage(string messageInterne, string imagePath, string message)
        {

            AddNewTextPage();

            Paragraph p = new Paragraph(messageInterne);
            p.Alignment = Element.ALIGN_CENTER;
            _docContent.Add(p);
            p = new Paragraph(Path.GetFileName(imagePath));
            p.Alignment = Element.ALIGN_CENTER;
            _docContent.Add(p);
            p = new Paragraph(message);
            p.Alignment = Element.ALIGN_CENTER;
            _docContent.Add(p);

            Rectangle rect = new Rectangle(_docContent.Left, _docContent.Bottom, _docContent.Right, _docContent.Top);
            rect.Border = Rectangle.BOX;
            rect.BorderWidth = 2;
            _writerContent.DirectContent.Rectangle(rect);
            
        }


        public int GetCurrentPageNumber()
        {
            return _writerContent.PageNumber;
        }

        public void WriteTitle(string text, int level = 0, bool center = false, bool underline = false, bool bold = false, bool italic = false)
        {
            var font = new Font(level == 0 ? fontTitrePrincipal : fontTitreSecondaire);
            var titre = new Paragraph(text, font);

            _border.Active = level == 0;
            font.SetStyle(Font.NORMAL);

            if (underline) font.SetStyle(font.Style | Font.UNDERLINE);
            if (bold) font.SetStyle(font.Style | Font.BOLD);
            if (italic) font.SetStyle(font.Style | Font.ITALIC);

            if (center) titre.Alignment = Element.ALIGN_CENTER;
            else
            {
                titre.FirstLineIndent = (level) * 20;
            }

            _docContent.Add(titre);
            _border.Active = false;
            if (level == 0) _docContent.Add(new Paragraph(" "));
            else WriteText(" ", null, null, null, false, 0);
        }

        public void WriteText(string text,
           string highlightedText = null,
           System.Drawing.Color? higlightedTextColor = null,
           System.Drawing.Color? backgroundHighlightedTextColor = null, bool bold = false)
        {

            WriteText(text, highlightedText, higlightedTextColor, backgroundHighlightedTextColor, bold, 0);
        }

        private void WriteText(string text,
            string highlightedText = null,
            System.Drawing.Color? higlightedTextColor = null,
            System.Drawing.Color? backgroundHighlightedTextColor = null, bool bold = false,
            int indentationLeft = 0)
        {
            var p = new Paragraph();

            var compareInfo = CultureInfo.InvariantCulture.CompareInfo;

            if (!String.IsNullOrWhiteSpace(highlightedText))
            {
                var fontHighlight = new Font(this.fontText);
                if(bold) fontHighlight.SetStyle(Font.BOLD);

                fontHighlight.Color = higlightedTextColor.HasValue ? new BaseColor(higlightedTextColor.Value) : BaseColor.BLACK;
                var backgroundHighlightColor = backgroundHighlightedTextColor.HasValue ? new BaseColor(backgroundHighlightedTextColor.Value) : BaseColor.YELLOW;
                var indexTextToHighlight = compareInfo.IndexOf(text,highlightedText, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);

                while (indexTextToHighlight >= 0)
                {
                    if (indexTextToHighlight == 0)
                    {
                        var c = new Chunk(text.Substring(0, highlightedText.Length), fontHighlight);
                        c.SetBackground(backgroundHighlightColor);
                        p.Add(c);
                        text = text.Substring(highlightedText.Length);
                    }
                    else
                    {
                        p.Add(new Chunk(text.Substring(0, indexTextToHighlight), this.fontText));
                        text = text.Substring(indexTextToHighlight);
                    }

                    indexTextToHighlight = compareInfo.IndexOf(text, highlightedText, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
                        //text.IndexOf(highlightedText, comparisonType: StringComparison.InvariantCultureIgnoreCase);
                }

               
            }
            
             p.Add(new Chunk(text, this.fontText));


            p.IndentationLeft = indentationLeft;
            _docContent.Add(p);
        }

        private void WriteInRectangle(string text,
           Rectangle rect, bool bold)
        {
            var p = new Paragraph();
            
            var font = new Font(this.fontText);
            
            if (bold) font.SetStyle(Font.BOLD);
            Chunk c = new Chunk(text, font);
            p.Add(c);

            ColumnText ct = new ColumnText(_writerContent.DirectContent);
            ct.SetSimpleColumn(rect);
            ct.AddElement(p);
            ct.Go();

        }

        public void AddNewPage()
        {
            AddNewTextPage();
        }

        private void AddNewTextPage()
        { 
            _docContent.SetMargins(36, 36, 36, 36);
            _docContent.NewPage();
           
        }

        // private void AddNewContentPage(PdfImportedPage importedPage = null)
        private void AddNewContentPage(Rectangle size = null)
        {
            
            if(size != null)
            {
                if (size.Height < size.Width)
                {
                    _docContent.SetPageSize(PageSize.A3.Rotate());
                }
            }
            else _docContent.SetPageSize(PageSize.A4);

            _docContent.SetMargins(0, 0, 0, 0);
            _docContent.NewPage();

        }

        public void WriteCatalogue(string text, string cote, string searchText)
        {
            var currentVertical = this._writerContent.GetVerticalPosition(false);

            if(currentVertical > 50)  WriteInRectangle(cote, new Rectangle(0 + _docContent.Left, currentVertical - 50, 50 + _docContent.Left, currentVertical), true);
            WriteText(text, searchText, null, System.Drawing.Color.Yellow, false, 50);
            if (currentVertical <= 50) WriteInRectangle(cote, new Rectangle(0 + _docContent.Left, _docContent.Top - 50, 50 + _docContent.Left, _docContent.Top), true);
        }
    }
}
