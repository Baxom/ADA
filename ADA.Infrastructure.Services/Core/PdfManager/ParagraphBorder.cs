using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADA.Infrastructure.Services.Core.PdfManager
{
    internal class ParagraphBorder : PdfPageEventHelper
    {
        public bool Active { get; set; }

        private float _startPosition;

        public override void OnParagraph(PdfWriter writer, Document document, float paragraphPosition)
        {
            _startPosition = paragraphPosition;
        }



        public override void OnParagraphEnd(PdfWriter writer, Document document, float paragraphPosition)
        {
            if (Active)
            {
                PdfContentByte cb = writer.DirectContentUnder;
                cb.Rectangle(document.Left, paragraphPosition - 4,
                    document.Right - document.Left,
                    _startPosition - paragraphPosition - 3);
                cb.Stroke();
            }
        }
    }

}
