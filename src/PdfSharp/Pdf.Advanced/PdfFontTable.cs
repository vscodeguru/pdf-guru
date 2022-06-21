using System.Diagnostics;
using System.Collections.Generic;
using PdfSharp.Drawing;

namespace PdfSharp.Pdf.Advanced
{
    internal enum FontType
    {
        TrueType = 1,

        Type0 = 2,
    }

    internal sealed class PdfFontTable : PdfResourceTable
    {
        public PdfFontTable(PdfDocument document)
            : base(document)
        { }

        public PdfFont GetFont(XFont font)
        {
            string selector = font.Selector;
            if (selector == null)
            {
                selector = ComputeKey(font);  
                font.Selector = selector;
            }
            PdfFont pdfFont;
            if (!_fonts.TryGetValue(selector, out pdfFont))
            {
                if (font.Unicode)
                    pdfFont = new PdfType0Font(Owner, font, font.IsVertical);
                else
                    pdfFont = new PdfTrueTypeFont(Owner, font);
                Debug.Assert(pdfFont.Owner == Owner);
                _fonts[selector] = pdfFont;
            }
            return pdfFont;
        }

        public PdfFont GetFont(string idName, byte[] fontData)
        {
            Debug.Assert(false);
            string selector = null;    
            PdfFont pdfFont;
            if (!_fonts.TryGetValue(selector, out pdfFont))
            {
                pdfFont = new PdfType0Font(Owner, idName, fontData, false);
                Debug.Assert(pdfFont.Owner == Owner);
                _fonts[selector] = pdfFont;
            }
            return pdfFont;
        }

        public PdfFont TryGetFont(string idName)
        {
            Debug.Assert(false);
            string selector = null;
            PdfFont pdfFont;
            _fonts.TryGetValue(selector, out pdfFont);
            return pdfFont;
        }

        internal static string ComputeKey(XFont font)
        {
            XGlyphTypeface glyphTypeface = font.GlyphTypeface;
            string key = glyphTypeface.Fontface.FullFaceName.ToLowerInvariant() +
                (glyphTypeface.IsBold ? "/b" : "") + (glyphTypeface.IsItalic ? "/i" : "") + font.Unicode;
            return key;
        }

        readonly Dictionary<string, PdfFont> _fonts = new Dictionary<string, PdfFont>();

        public void PrepareForSave()
        {
            foreach (PdfFont font in _fonts.Values)
                font.PrepareForSave();
        }
    }
}
