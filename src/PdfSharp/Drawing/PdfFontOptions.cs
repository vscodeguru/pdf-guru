using System;
using PdfSharp.Pdf;

namespace PdfSharp.Drawing
{
    public class XPdfFontOptions
    {
        internal XPdfFontOptions() { }

        [Obsolete("Must not specify an embedding option anymore.")]
        public XPdfFontOptions(PdfFontEncoding encoding, PdfFontEmbedding embedding)
        {
            _fontEncoding = encoding;
        }

        public XPdfFontOptions(PdfFontEncoding encoding)
        {
            _fontEncoding = encoding;
        }

        [Obsolete("Must not specify an embedding option anymore.")]
        public XPdfFontOptions(PdfFontEmbedding embedding)
        {
            _fontEncoding = PdfFontEncoding.WinAnsi;
        }

        public PdfFontEmbedding FontEmbedding
        {
            get { return PdfFontEmbedding.Always; }
        }

        public PdfFontEncoding FontEncoding
        {
            get { return _fontEncoding; }
        }
        readonly PdfFontEncoding _fontEncoding;

        public static XPdfFontOptions WinAnsiDefault
        {
            get { return new XPdfFontOptions(PdfFontEncoding.WinAnsi); }
        }

        public static XPdfFontOptions UnicodeDefault
        {
            get { return new XPdfFontOptions(PdfFontEncoding.Unicode); }
        }
    }
}
