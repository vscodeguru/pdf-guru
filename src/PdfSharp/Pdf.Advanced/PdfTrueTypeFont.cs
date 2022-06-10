using System.Diagnostics;
using PdfSharp.Fonts;
using PdfSharp.Fonts.OpenType;
using PdfSharp.Drawing;
using PdfSharp.Pdf.Filters;

namespace PdfSharp.Pdf.Advanced
{
    internal class PdfTrueTypeFont : PdfFont
    {
        public PdfTrueTypeFont(PdfDocument document)
            : base(document)
        { }

        public PdfTrueTypeFont(PdfDocument document, XFont font)
            : base(document)
        {
            Elements.SetName(Keys.Type, "/Font");
            Elements.SetName(Keys.Subtype, "/TrueType");

            OpenTypeDescriptor ttDescriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptorFor(font);
            FontDescriptor = new PdfFontDescriptor(document, ttDescriptor);
            _fontOptions = font.PdfOptions;
            Debug.Assert(_fontOptions != null);

            _cmapInfo = new CMapInfo(ttDescriptor);

            BaseFont = font.GlyphTypeface.GetBaseName();
     
            if (_fontOptions.FontEmbedding == PdfFontEmbedding.Always)
                BaseFont = PdfFont.CreateEmbeddedFontSubsetName(BaseFont);
            FontDescriptor.FontName = BaseFont;

            Debug.Assert(_fontOptions.FontEncoding == PdfFontEncoding.WinAnsi);
            if (!IsSymbolFont)
                Encoding = "/WinAnsiEncoding";

            Owner._irefTable.Add(FontDescriptor);
            Elements[Keys.FontDescriptor] = FontDescriptor.Reference;

            FontEncoding = font.PdfOptions.FontEncoding;
        }

        XPdfFontOptions FontOptions
        {
            get { return _fontOptions; }
        }
        readonly XPdfFontOptions _fontOptions;

        public string BaseFont
        {
            get { return Elements.GetName(Keys.BaseFont); }
            set { Elements.SetName(Keys.BaseFont, value); }
        }

        public int FirstChar
        {
            get { return Elements.GetInteger(Keys.FirstChar); }
            set { Elements.SetInteger(Keys.FirstChar, value); }
        }

        public int LastChar
        {
            get { return Elements.GetInteger(Keys.LastChar); }
            set { Elements.SetInteger(Keys.LastChar, value); }
        }

        public PdfArray Widths
        {
            get { return (PdfArray)Elements.GetValue(Keys.Widths, VCF.Create); }
        }

        public string Encoding
        {
            get { return Elements.GetName(Keys.Encoding); }
            set { Elements.SetName(Keys.Encoding, value); }
        }

        internal override void PrepareForSave()
        {
            base.PrepareForSave();

            OpenTypeFontface subSet = FontDescriptor._descriptor.FontFace.CreateFontSubSet(_cmapInfo.GlyphIndices, false);
            byte[] fontData = subSet.FontSource.Bytes;

            PdfDictionary fontStream = new PdfDictionary(Owner);
            Owner.Internals.AddObject(fontStream);
            FontDescriptor.Elements[PdfFontDescriptor.Keys.FontFile2] = fontStream.Reference;

            fontStream.Elements["/Length1"] = new PdfInteger(fontData.Length);
            if (!Owner.Options.NoCompression)
            {
                fontData = Filtering.FlateDecode.Encode(fontData, _document.Options.FlateEncodeMode);
                fontStream.Elements["/Filter"] = new PdfName("/FlateDecode");
            }
            fontStream.Elements["/Length"] = new PdfInteger(fontData.Length);
            fontStream.CreateStream(fontData);

            FirstChar = 0;
            LastChar = 255;
            PdfArray width = Widths;
            for (int idx = 0; idx < 256; idx++)
                width.Elements.Add(new PdfInteger(FontDescriptor._descriptor.Widths[idx]));
        }

        public new sealed class Keys : PdfFont.Keys
        {
            [KeyInfo(KeyType.Name | KeyType.Required, FixedValue = "Font")]
            public new const string Type = "/Type";

            [KeyInfo(KeyType.Name | KeyType.Required)]
            public new const string Subtype = "/Subtype";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string Name = "/Name";

            [KeyInfo(KeyType.Name | KeyType.Required)]
            public new const string BaseFont = "/BaseFont";

            [KeyInfo(KeyType.Integer)]
            public const string FirstChar = "/FirstChar";

            [KeyInfo(KeyType.Integer)]
            public const string LastChar = "/LastChar";

            [KeyInfo(KeyType.Array, typeof(PdfArray))]
            public const string Widths = "/Widths";

            [KeyInfo(KeyType.Dictionary | KeyType.MustBeIndirect, typeof(PdfFontDescriptor))]
            public new const string FontDescriptor = "/FontDescriptor";

            [KeyInfo(KeyType.Name | KeyType.Dictionary)]
            public const string Encoding = "/Encoding";

            [KeyInfo(KeyType.Stream | KeyType.Optional)]
            public const string ToUnicode = "/ToUnicode";

            internal static DictionaryMeta Meta
            {
                get { return _meta ?? (_meta = CreateMeta(typeof(Keys))); }
            }
            static DictionaryMeta _meta;
        }

        internal override DictionaryMeta Meta
        {
            get { return Keys.Meta; }
        }
    }
}
