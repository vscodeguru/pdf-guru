using System.IO;
using PdfSharp.Pdf.Content.Objects;

namespace PdfSharp.Pdf.Content
{
    public static class ContentReader
    {
        static public CSequence ReadContent(PdfPage page)
        {
            CParser parser = new CParser(page);
            CSequence sequence = parser.ReadContent();

            return sequence;
        }

        static public CSequence ReadContent(byte[] content)
        {
            CParser parser = new CParser(content);
            CSequence sequence = parser.ReadContent();
            return sequence;
        }

        static public CSequence ReadContent(MemoryStream content)
        {
            CParser parser = new CParser(content);
            CSequence sequence = parser.ReadContent();
            return sequence;
        }
    }
}
