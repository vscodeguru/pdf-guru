namespace PdfSharp.Pdf.Annotations
{
    internal sealed class PdfGenericAnnotation : PdfAnnotation
    {
        public PdfGenericAnnotation(PdfDictionary dict)
            : base(dict)
        { }

        internal new class Keys : PdfAnnotation.Keys
        {
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
