namespace PdfSharp.Pdf.AcroForms
{
    public sealed class PdfPushButtonField : PdfButtonField
    {
        internal PdfPushButtonField(PdfDocument document)
            : base(document)
        {
            _document = document;
        }

        internal PdfPushButtonField(PdfDictionary dict)
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
