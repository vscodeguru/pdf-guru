namespace PdfSharp.Pdf.Annotations
{
    internal sealed class PdfWidgetAnnotation : PdfAnnotation
    {
        public PdfWidgetAnnotation()
        {
            Initialize();
        }

        public PdfWidgetAnnotation(PdfDocument document)
            : base(document)
        {
            Initialize();
        }

        void Initialize()
        {
            Elements.SetName(Keys.Subtype, "/Widget");
        }

        internal new class Keys : PdfAnnotation.Keys
        {
            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string H = "/H";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional)]
            public const string MK = "/MK";

            public static DictionaryMeta Meta
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
