
namespace PdfSharp.Pdf.Advanced
{
    public sealed class PdfTilingPattern : PdfDictionaryWithContentStream
    {
        public PdfTilingPattern(PdfDocument document)
            : base(document)
        {
            Elements.SetName(Keys.Type, "/Pattern");
            Elements[Keys.PatternType] = new PdfInteger(1);
        }

        internal sealed new class Keys : PdfDictionaryWithContentStream.Keys
        {
            [KeyInfo(KeyType.Name | KeyType.Required)]
            public const string Type = "/Type";

            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public const string PatternType = "/PatternType";

            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public const string PaintType = "/PaintType";

            [KeyInfo(KeyType.Integer | KeyType.Required)]
            public const string TilingType = "/TilingType";

            [KeyInfo(KeyType.Rectangle | KeyType.Optional)]
            public const string BBox = "/BBox";

            [KeyInfo(KeyType.Real | KeyType.Required)]
            public const string XStep = "/XStep";

            [KeyInfo(KeyType.Real | KeyType.Required)]
            public const string YStep = "/YStep";

            [KeyInfo(KeyType.Dictionary | KeyType.Required)]
            public new const string Resources = "/Resources";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string Matrix = "/Matrix";

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
