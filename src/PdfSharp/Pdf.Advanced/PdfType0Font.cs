using System.Diagnostics;
using System.Text;
using PdfSharp.Fonts;
using PdfSharp.Fonts.OpenType;
using PdfSharp.Drawing;

namespace PdfSharp.Pdf.Advanced
{
    internal sealed class PdfType0Font : PdfFont
    {
        public PdfType0Font(PdfDocument document)
            : base(document)
        { }

        public PdfType0Font(PdfDocument document, XFont font, bool vertical)
            : base(document)
        {
            Elements.SetName(Keys.Type, "/Font");
            Elements.SetName(Keys.Subtype, "/Type0");
            Elements.SetName(Keys.Encoding, vertical ? "/Identity-V" : "/Identity-H");

            OpenTypeDescriptor ttDescriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptorFor(font);
            FontDescriptor = new PdfFontDescriptor(document, ttDescriptor);
            _fontOptions = font.PdfOptions;
            Debug.Assert(_fontOptions != null);

            _cmapInfo = new CMapInfo(ttDescriptor);
            _descendantFont = new PdfCIDFont(document, FontDescriptor, font);
            _descendantFont.CMapInfo = _cmapInfo;

            _toUnicode = new PdfToUnicodeMap(document, _cmapInfo);
            document.Internals.AddObject(_toUnicode);
            Elements.Add(Keys.ToUnicode, _toUnicode);

            BaseFont = font.GlyphTypeface.GetBaseName();
            BaseFont = PdfFont.CreateEmbeddedFontSubsetName(BaseFont);

            FontDescriptor.FontName = BaseFont;
            _descendantFont.BaseFont = BaseFont;

            PdfArray descendantFonts = new PdfArray(document);
            Owner._irefTable.Add(_descendantFont);
            descendantFonts.Elements.Add(_descendantFont.Reference);
            Elements[Keys.DescendantFonts] = descendantFonts;
        }

        public PdfType0Font(PdfDocument document, string idName, byte[] fontData, bool vertical)
            : base(document)
        {
            Elements.SetName(Keys.Type, "/Font");
            Elements.SetName(Keys.Subtype, "/Type0");
            Elements.SetName(Keys.Encoding, vertical ? "/Identity-V" : "/Identity-H");

            OpenTypeDescriptor ttDescriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptor(idName, fontData);
            FontDescriptor = new PdfFontDescriptor(document, ttDescriptor);
            _fontOptions = new XPdfFontOptions(PdfFontEncoding.Unicode);
            Debug.Assert(_fontOptions != null);

            _cmapInfo = new CMapInfo(ttDescriptor);
            _descendantFont = new PdfCIDFont(document, FontDescriptor, fontData);
            _descendantFont.CMapInfo = _cmapInfo;

            _toUnicode = new PdfToUnicodeMap(document, _cmapInfo);
            document.Internals.AddObject(_toUnicode);
            Elements.Add(Keys.ToUnicode, _toUnicode);

            BaseFont = ttDescriptor.FontName;

            if (!BaseFont.Contains("+"))     
                BaseFont = CreateEmbeddedFontSubsetName(BaseFont);

            FontDescriptor.FontName = BaseFont;
            _descendantFont.BaseFont = BaseFont;

            PdfArray descendantFonts = new PdfArray(document);
            Owner._irefTable.Add(_descendantFont);
            descendantFonts.Elements.Add(_descendantFont.Reference);
            Elements[Keys.DescendantFonts] = descendantFonts;
        }

        XPdfFontOptions FontOptions
        {
            get { return _fontOptions; }
        }
        XPdfFontOptions _fontOptions;

        public string BaseFont
        {
            get { return Elements.GetName(Keys.BaseFont); }
            set { Elements.SetName(Keys.BaseFont, value); }
        }

        internal PdfCIDFont DescendantFont
        {
            get { return _descendantFont; }
        }
        readonly PdfCIDFont _descendantFont;

        internal override void PrepareForSave()
        {
            base.PrepareForSave();

            OpenTypeDescriptor descriptor = (OpenTypeDescriptor)FontDescriptor._descriptor;
            StringBuilder w = new StringBuilder("[");
            if (_cmapInfo != null)
            {
                int[] glyphIndices = _cmapInfo.GetGlyphIndices();
                int count = glyphIndices.Length;
                int[] glyphWidths = new int[count];

                for (int idx = 0; idx < count; idx++)
                    glyphWidths[idx] = descriptor.GlyphIndexToPdfWidth(glyphIndices[idx]);

                for (int idx = 0; idx < count; idx++)
                    w.AppendFormat("{0}[{1}]", glyphIndices[idx], glyphWidths[idx]);
                w.Append("]");
                _descendantFont.Elements.SetValue(PdfCIDFont.Keys.W, new PdfLiteral(w.ToString()));

            }
            _descendantFont.PrepareForSave();
            _toUnicode.PrepareForSave();
        }

        public new sealed class Keys : PdfFont.Keys
        {
            [KeyInfo(KeyType.Name | KeyType.Required, FixedValue = "Font")]
            public new const string Type = "/Type";

            [KeyInfo(KeyType.Name | KeyType.Required)]
            public new const string Subtype = "/Subtype";

            [KeyInfo(KeyType.Name | KeyType.Required)]
            public new const string BaseFont = "/BaseFont";

            [KeyInfo(KeyType.StreamOrName | KeyType.Required)]
            public const string Encoding = "/Encoding";

            [KeyInfo(KeyType.Array | KeyType.Required)]
            public const string DescendantFonts = "/DescendantFonts";

            [KeyInfo(KeyType.Stream | KeyType.Optional)]
            public const string ToUnicode = "/ToUnicode";

            internal static DictionaryMeta Meta
            {
                get
                {
                    if (Keys._meta == null)
                        Keys._meta = CreateMeta(typeof(Keys));
                    return Keys._meta;
                }
            }
            static DictionaryMeta _meta;
        }

        internal override DictionaryMeta Meta
        {
            get { return Keys.Meta; }
        }
    }
}
