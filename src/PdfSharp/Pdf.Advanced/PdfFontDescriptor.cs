using System;
using PdfSharp.Fonts.OpenType;

namespace PdfSharp.Pdf.Advanced
{
    [Flags]
    enum PdfFontDescriptorFlags
    {
        FixedPitch = 1 << 0,

        Serif = 1 << 1,

        Symbolic = 1 << 2,

        Script = 1 << 3,

        Nonsymbolic = 1 << 5,

        Italic = 1 << 6,

        AllCap = 1 << 16,

        SmallCap = 1 << 17,

        ForceBold = 1 << 18,
    }

    public sealed class PdfFontDescriptor : PdfDictionary
    {
        internal PdfFontDescriptor(PdfDocument document, OpenTypeDescriptor descriptor)
            : base(document)
        {
            _descriptor = descriptor;
            Elements.SetName(Keys.Type, "/FontDescriptor");

            Elements.SetInteger(Keys.Ascent, _descriptor.DesignUnitsToPdf(_descriptor.Ascender));
            Elements.SetInteger(Keys.CapHeight, _descriptor.DesignUnitsToPdf(_descriptor.CapHeight));
            Elements.SetInteger(Keys.Descent, _descriptor.DesignUnitsToPdf(_descriptor.Descender));
            Elements.SetInteger(Keys.Flags, (int)FlagsFromDescriptor(_descriptor));
            Elements.SetRectangle(Keys.FontBBox, new PdfRectangle(
              _descriptor.DesignUnitsToPdf(_descriptor.XMin),
              _descriptor.DesignUnitsToPdf(_descriptor.YMin),
              _descriptor.DesignUnitsToPdf(_descriptor.XMax),
              _descriptor.DesignUnitsToPdf(_descriptor.YMax)));
            Elements.SetReal(Keys.ItalicAngle, _descriptor.ItalicAngle);
            Elements.SetInteger(Keys.StemV, _descriptor.StemV);
            Elements.SetInteger(Keys.XHeight, _descriptor.DesignUnitsToPdf(_descriptor.XHeight));
        }

        internal OpenTypeDescriptor _descriptor;

        public string FontName
        {
            get { return Elements.GetName(Keys.FontName); }
            set { Elements.SetName(Keys.FontName, value); }
        }

        public bool IsSymbolFont
        {
            get { return _isSymbolFont; }
        }
        bool _isSymbolFont;

        PdfFontDescriptorFlags FlagsFromDescriptor(OpenTypeDescriptor descriptor)
        {
            PdfFontDescriptorFlags flags = 0;
            _isSymbolFont = descriptor.FontFace.cmap.symbol;
            flags |= descriptor.FontFace.cmap.symbol ? PdfFontDescriptorFlags.Symbolic : PdfFontDescriptorFlags.Nonsymbolic;
            return flags;
        }

        public sealed class Keys : KeysBase
        {
            [KeyInfo(KeyType.Name | KeyType.Required, FixedValue = "FontDescriptor")]
            public const string Type = "/Type";

            [KeyInfo(KeyType.Name | KeyType.Required)]
            public const string FontName = "/FontName";

            [KeyInfo(KeyType.String | KeyType.Optional)]
            public const string FontFamily = "/FontFamily";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string FontStretch = "/FontStretch";

            [KeyInfo(KeyType.Real | KeyType.Optional)]
            public const string FontWeight = "/FontWeight";

            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public const string Flags = "/Flags";

            [KeyInfo(KeyType.Rectangle | KeyType.Required)]
            public const string FontBBox = "/FontBBox";

            [KeyInfo(KeyType.Real | KeyType.Required)]
            public const string ItalicAngle = "/ItalicAngle";

            [KeyInfo(KeyType.Real | KeyType.Required)]
            public const string Ascent = "/Ascent";

            [KeyInfo(KeyType.Real | KeyType.Required)]
            public const string Descent = "/Descent";

            [KeyInfo(KeyType.Real | KeyType.Optional)]
            public const string Leading = "/Leading";

            [KeyInfo(KeyType.Real | KeyType.Required)]
            public const string CapHeight = "/CapHeight";

            [KeyInfo(KeyType.Real | KeyType.Optional)]
            public const string XHeight = "/XHeight";

            [KeyInfo(KeyType.Real | KeyType.Required)]
            public const string StemV = "/StemV";

            [KeyInfo(KeyType.Real | KeyType.Optional)]
            public const string StemH = "/StemH";

            [KeyInfo(KeyType.Real | KeyType.Optional)]
            public const string AvgWidth = "/AvgWidth";

            [KeyInfo(KeyType.Real | KeyType.Optional)]
            public const string MaxWidth = "/MaxWidth";

            [KeyInfo(KeyType.Real | KeyType.Optional)]
            public const string MissingWidth = "/MissingWidth";

            [KeyInfo(KeyType.Stream | KeyType.Optional)]
            public const string FontFile = "/FontFile";

            [KeyInfo(KeyType.Stream | KeyType.Optional)]
            public const string FontFile2 = "/FontFile2";

            [KeyInfo(KeyType.Stream | KeyType.Optional)]
            public const string FontFile3 = "/FontFile3";

            [KeyInfo(KeyType.String | KeyType.Optional)]
            public const string CharSet = "/CharSet";

            internal static DictionaryMeta Meta
            {
                get
                {
                    if (_meta == null)
                        _meta = CreateMeta(typeof(Keys));
                    return _meta;
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
