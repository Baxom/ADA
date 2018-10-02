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
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ADA.Infrastructure.Services.Core.PdfManager
{
    public class iTextSharpPdfManager : IPdfManager, IDisposable
    {
        Rectangle pageSize = PageSize.A4;
        bool _badInitialisation = true;
        Stream _outputContent = null;

        Document _docContent = null;
        PdfWriter _writerContent = null;
        ParagraphBorder _border = new ParagraphBorder();

        List<PdfReader> _readers = null;

        Font fontTitrePrincipal = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false), 16);
        Font fontTitreSecondaire = new Font(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false), 13);
        Font fontText = new Font(BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, false), 12);
        Font fontStamp = new Font(BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, false), 12, (int)Font.NORMAL, BaseColor.BLUE);
        Font fontAnchor = new Font(BaseFont.CreateFont(BaseFont.TIMES_ITALIC, BaseFont.CP1252, false), 12, (int)Font.NORMAL, BaseColor.RED);

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
            _docContent = new Document();
            _writerContent = PdfWriter.GetInstance(_docContent, _outputContent);
            _writerContent.PageEvent = _border;
            _writerContent.CloseStream = false;
            _docContent.Open();
            _writerContent.SetLinearPageMode();

        }

        /// <summary>
        /// Factory
        /// </summary>
        /// <returns></returns>
        public IPdfManager Create(Stream stream)
        {
            return new iTextSharpPdfManager(stream);
        }

        public int AddPdf(string pdfPath, string stampString = null, string fileNotFoundMessage = null, List<int> pageIndexToPrint = null, bool autoResize = true)
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
                
                AddNewContentPage(reader.GetCropBox(i), reader.GetPageRotation(i), autoResize);


                PdfImportedPage page = _writerContent.GetImportedPage(reader, i);
                page.BoundingBox = reader.GetCropBox(i);

                _writerContent.DirectContent.AddTemplate(page, CreateTransform(page, page.Rotation, reader.GetPageSize(i), autoResize));
              

                CreateStamp(stampString);
            }

            // traitement parge inexistante
            pageIndexToPrint.Where(b => b > reader.NumberOfPages).ToList().ForEach(b =>
            {
                AddNewContentPage();
                CreateMissingFilePage(String.Format("La page {0} n'existe pas dans le fichier Pdf", b), pdfPath, fileNotFoundMessage);
            });

            return pageIndexToPrint.Any() ? pageIndexToPrint.Count : reader.NumberOfPages;
        }

        private void CreateDestination()
        {
            _writerContent.AddNamedDestination("p" + _writerContent.CurrentPageNumber.ToString(), _writerContent.CurrentPageNumber,
                new PdfDestination(PdfDestination.XYZ, 0, _writerContent.PageSize.Top, 0));
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

        private AffineTransform CreateTransform(PdfImportedPage page, int rotation, Rectangle initialPageSize, bool autoResize)
        {

            var docWidth = _docContent.Right - _docContent.Left;
            var docHeight = _docContent.Top - _docContent.Bottom;

            AffineTransform rotate = AffineTransform.GetRotateInstance((Math.PI * (360 - rotation)) / 180, 0, 0);
            AffineTransform scale = AffineTransform.GetScaleInstance(docWidth / page.Width, docHeight / page.Height);
            AffineTransform translateMargin = AffineTransform.GetTranslateInstance(_docContent.LeftMargin - page.BoundingBox.Left + initialPageSize.Left,
                _docContent.BottomMargin - page.BoundingBox.Bottom + initialPageSize.Bottom);

            AffineTransform translateRotate = AffineTransform.GetTranslateInstance(0, 0);
            if (rotation % 180 == 90)
            {
                scale = AffineTransform.GetScaleInstance(docWidth / page.Height, docHeight / page.Width);

                translateRotate = AffineTransform.GetTranslateInstance((rotation % 360 == 270) ? page.Height : 0,
                    (rotation % 360 == 90) ? page.Width : 0);
            }
            else if (rotation % 360 == 180)
            {
                translateRotate = AffineTransform.GetTranslateInstance(page.Width, page.Height); ;
            }


            AffineTransform transform = AffineTransform.GetTranslateInstance(0, 0);

            if (autoResize) transform.preConcatenate(translateMargin);
            transform.preConcatenate(rotate);
            transform.preConcatenate(translateRotate);
            if (autoResize) transform.preConcatenate(scale);

            return transform;
        }

        public void Close()
        {
            AssertGoodInitialization();
            _docContent.Close();
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

                numberOfPage = CreateTableOfContent(toc, writer, doc, false);
            }

            return numberOfPage;

        }

        public void Dispose()
        {

        }

        public int AddTableOfContent(PdfTableOfContent toc)
        {
            AssertGoodInitialization();
            return CreateTableOfContent(toc, _writerContent, _docContent, true);
        }

        private int CreateTableOfContent(PdfTableOfContent toc, PdfWriter writer, Document doc, bool reorderPage)
        {
            var numberOfPageBeforeInsertTOC = writer.PageNumber;
            doc.SetPageSize(PageSize.A4);
            doc.SetMargins(36, 36, 36, 36);
            doc.NewPage();
            Paragraph p;
            PdfAction action = new PdfAction(PdfAction.FIRSTPAGE);
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

              //  action = PdfAction.GotoLocalPage("p" + content.RealPage, new PdfDestination(PdfDestination.XYZ, 0f, PageSize.A4.Top, 0f), writer);
                action = PdfAction.GotoLocalPage("p" + content.RealPage, false);
                p = new Paragraph(content.Titre, fontText);
                p.Add(new Chunk(new DottedLineSeparator()));
                p.Add(new Chunk(content.PageNumber.ToString(), fontText));

                float topY = writer.GetVerticalPosition(false);

                doc.Add(p);

                float bottomYY = writer.GetVerticalPosition(false);

                if (bottomYY > topY) // on a changé de page
                {
                    topY = _docContent.Top;
                }

                link = new PdfAnnotation(writer, doc.Left, bottomYY, doc.Right, topY, action);
                writer.AddAnnotation(link);
            }

            var numberOfPage = writer.PageNumber;

            if (reorderPage)
            {
                doc.NewPage();
                var reorderArray = Enumerable.Range(numberOfPageBeforeInsertTOC + 1, numberOfPage - numberOfPageBeforeInsertTOC)
                        .Concat(Enumerable.Range(1, numberOfPageBeforeInsertTOC)).ToArray();
                writer.ReorderPages(reorderArray);
            }

            return numberOfPage;
        }

        public int AddImage(string imagePath, string titre = null, string stampString = null, string fileNotFoundMessage = null)
        {
            AssertGoodInitialization();

            if (!File.Exists(imagePath))
            {
                CreateMissingFilePage("Photo introuvable", imagePath, fileNotFoundMessage);
            }
            else
            {

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
           List<KeyValuePair<string, bool>> highlightedTerms = null,
           System.Drawing.Color? higlightedTextColor = null,
           System.Drawing.Color? backgroundHighlightedTextColor = null, bool bold = false)
        {

            WriteText(text, highlightedTerms, higlightedTextColor, backgroundHighlightedTextColor, bold, 0);
        }

        private void WriteText(string text,
            List<KeyValuePair<string, bool>> highlightedTerms = null,
            System.Drawing.Color? higlightedTextColor = null,
            System.Drawing.Color? backgroundHighlightedTextColor = null, bool bold = false,
            int indentationLeft = 0)
        {
            var p = new Paragraph();
            string pattern = @"^[a-zA-Z]+$";
            Regex regexIsALetter = new Regex(pattern);
            var compareInfo = CultureInfo.InvariantCulture.CompareInfo;

            if (highlightedTerms != null)
            {
                foreach (var kvp in highlightedTerms)
                {
                    var highlightedText = kvp.Key;
                    var wordBoundary = kvp.Value;

                    if (!String.IsNullOrWhiteSpace(highlightedText))
                    {
                        var fontHighlight = new Font(this.fontText);
                        if (bold) fontHighlight.SetStyle(Font.BOLD);

                        fontHighlight.Color = higlightedTextColor.HasValue ? new BaseColor(higlightedTextColor.Value) : BaseColor.BLACK;
                        var backgroundHighlightColor = backgroundHighlightedTextColor.HasValue ? new BaseColor(backgroundHighlightedTextColor.Value) : BaseColor.YELLOW;
                        var indexTextToHighlight = compareInfo.IndexOf(text, highlightedText, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);

                        while (indexTextToHighlight >= 0)
                        {



                            if (indexTextToHighlight == 0)
                            {
                                bool isWordComplete = false;

                                isWordComplete = (indexTextToHighlight + highlightedText.Length == text.Length)
                                    || !regexIsALetter.IsMatch(text.ElementAt(indexTextToHighlight + highlightedText.Length).ToString());

                                if (wordBoundary && isWordComplete || !wordBoundary)
                                {
                                    var c = new Chunk(text.Substring(0, highlightedText.Length), fontHighlight);
                                    c.SetBackground(backgroundHighlightColor);
                                    p.Add(c);
                                    text = text.Substring(highlightedText.Length);
                                }
                                else
                                {
                                    p.Add(new Chunk(text.Substring(0, highlightedText.Length), this.fontText));
                                    text = text.Substring(highlightedText.Length);
                                }


                            }
                            else
                            {
                                p.Add(new Chunk(text.Substring(0, indexTextToHighlight), this.fontText));
                                text = text.Substring(indexTextToHighlight);
                            }

                            indexTextToHighlight = compareInfo.IndexOf(text, highlightedText, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);
                        }


                    }
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
            _docContent.SetPageSize(PageSize.A4);
            _docContent.SetMargins(36, 36, 36, 36);
            _docContent.NewPage();

        }

        private void AddNewContentPage(Rectangle size = null, int rotation = 0, bool autoResize = true)
        {
            bool mustRotate = rotation % 180 == 90;
            size = size ?? PageSize.A4;
            Rectangle pageSize = size;

            if (autoResize)
            {
                if (size.Width < size.Height && (size.Width < PageSize.A4.Width || size.Width < PageSize.A4.Height)) pageSize = PageSize.A4;
                if (size.Width >= size.Height && (size.Width < PageSize.A4.Rotate().Width || size.Width < PageSize.A4.Rotate().Height)) pageSize = PageSize.A4.Rotate();

                if (size.Height >= 1000) pageSize = PageSize.A3;
                if (size.Width >= 1000 && size.Height < size.Width ) pageSize = PageSize.A3.Rotate();
            }
            //if(size.Width > size.Height)
            if (mustRotate)
                pageSize = pageSize.Rotate();

        
            if(pageSize.Rotation >= 180)
            {
                pageSize.Rotation -= 180;
            }

            _docContent.SetPageSize(pageSize);
            _docContent.SetMargins(0, 0, 0, 0);
            _docContent.NewPage();
            CreateDestination();

        }

        public void WriteCatalogue(string text, string cote, List<KeyValuePair<string, bool>> searchTerms)
        {
            var currentVertical = this._writerContent.GetVerticalPosition(false);

            if (currentVertical > 50) WriteInRectangle(cote, new Rectangle(0 + _docContent.Left, currentVertical - 50, 50 + _docContent.Left, currentVertical), true);
            WriteText(text, searchTerms, null, System.Drawing.Color.Yellow, false, 50);
            if (currentVertical <= 50) WriteInRectangle(cote, new Rectangle(0 + _docContent.Left, _docContent.Top - 50, 50 + _docContent.Left, _docContent.Top), true);
        }
    }
}
