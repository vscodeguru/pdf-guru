
using PdfSharp.Drawing;
using PdfSharp.Pdf.Filters;
using PdfSharp.Fonts.OpenType;

namespace PdfSharp.Pdf.Advanced
{
    internal class PdfCIDFont : PdfFont
    {
        public PdfCIDFont(PdfDocument document)
            : base(document)
        { }

        public PdfCIDFont(PdfDocument document, PdfFontDescriptor fontDescriptor, XFont font)
            : base(document)
        {
            Elements.SetName(Keys.Type, "/Font");
            Elements.SetName(Keys.Subtype, "/CIDFontType2");
            PdfDictionary cid = new PdfDictionary();
            cid.Elements.SetString("/Ordering", "Identity");
            cid.Elements.SetString("/Registry", "Adobe");
            cid.Elements.SetInteger("/Supplement", 0);
            Elements.SetValue(Keys.CIDSystemInfo, cid);

            FontDescriptor = fontDescriptor;
            Owner._irefTable.Add(fontDescriptor);
            Elements[Keys.FontDescriptor] = fontDescriptor.Reference;

            FontEncoding = font.PdfOptions.FontEncoding;
        }

        public PdfCIDFont(PdfDocument document, PdfFontDescriptor fontDescriptor, byte[] fontData)
            : base(document)
        {
            Elements.SetName(Keys.Type, "/Font");
            Elements.SetName(Keys.Subtype, "/CIDFontType2");
            PdfDictionary cid = new PdfDictionary();
            cid.Elements.SetString("/Ordering", "Identity");
            cid.Elements.SetString("/Registry", "Adobe");
            cid.Elements.SetInteger("/Supplement", 0);
            Elements.SetValue(Keys.CIDSystemInfo, cid);

            FontDescriptor = fontDescriptor;
            Owner._irefTable.Add(fontDescriptor);
            Elements[Keys.FontDescriptor] = fontDescriptor.Reference;

            FontEncoding = PdfFontEncoding.Unicode;
        }

        public string BaseFont
        {
            get { return Elements.GetName(Keys.BaseFont); }
            set { Elements.SetName(Keys.BaseFont, value); }
        }

        internal override void PrepareForSave()
        {
            base.PrepareForSave();

            OpenTypeFontface subSet = null;
            if (FontDescriptor._descriptor.FontFace.loca == null)
                subSet = FontDescriptor._descriptor.FontFace;
            else
                subSet = FontDescriptor._descriptor.FontFace.CreateFontSubSet(_cmapInfo.GlyphIndices, true);
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
        }

        public new sealed class Keys : PdfFont.Keys
        {
            [KeyInfo(KeyType.Name | KeyType.Required, FixedValue = "Font")]
            public new const string Type = "/Type";

            [KeyInfo(KeyType.Name | KeyType.Required)]
            public new const string Subtype = "/Subtype";

            [KeyInfo(KeyType.Name | KeyType.Required)]
            public new const string BaseFont = "/BaseFont";

            [KeyInfo(KeyType.Dictionary | KeyType.Required)]
            public const string CIDSystemInfo = "/CIDSystemInfo";

            [KeyInfo(KeyType.Dictionary | KeyType.MustBeIndirect, typeof(PdfFontDescriptor))]
            public new const string FontDescriptor = "/FontDescriptor";

            [KeyInfo(KeyType.Integer)]
            public const string DW = "/DW";

            [KeyInfo(KeyType.Array, typeof(PdfArray))]
            public const string W = "/W";

            [KeyInfo(KeyType.Array)]
            public const string DW2 = "/DW2";

            [KeyInfo(KeyType.Array, typeof(PdfArray))]
            public const string W2 = "/W2";

            [KeyInfo(KeyType.Dictionary | KeyType.StreamOrName)]
            public const string CIDToGIDMap = "/CIDToGIDMap";

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
