namespace PdfSharp.Pdf.AcroForms
{
    public sealed class PdfGenericField : PdfAcroField
    {
        internal PdfGenericField(PdfDocument document)
            : base(document)
        { }

        internal PdfGenericField(PdfDictionary dict)
            : base(dict)
        { }

        public new class Keys : PdfAcroField.Keys
        {
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
