namespace PdfSharp.Pdf.Advanced
{
    public sealed class PdfTransparencyGroupAttributes : PdfGroupAttributes
    {
        internal PdfTransparencyGroupAttributes(PdfDocument thisDocument)
            : base(thisDocument)
        {
            Elements.SetName(Keys.S, "/Transparency");
        }

        public sealed new class Keys : PdfGroupAttributes.Keys
        {
            [KeyInfo(KeyType.NameOrArray | KeyType.Optional)]
            public const string CS = "/CS";

            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string I = "/I";

            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string K = "/K";

            internal static new DictionaryMeta Meta
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
