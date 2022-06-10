

using PdfSharp.Drawing;

namespace PdfSharp.Pdf.Advanced
{
    internal interface IContentStream
    {
        PdfResources Resources { get; }

        string GetFontName(XFont font, out PdfFont pdfFont);

        string GetFontName(string idName, byte[] fontData, out PdfFont pdfFont);

        string GetImageName(XImage image);

        string GetFormName(XForm form);
    }
}