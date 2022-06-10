namespace PdfSharp.Pdf.AcroForms
{
    public sealed class PdfListBoxField : PdfChoiceField
    {
        internal PdfListBoxField(PdfDocument document)
            : base(document)
        { }

        internal PdfListBoxField(PdfDictionary dict)
            : base(dict)
        { }

        public int SelectedIndex
        {
            get
            {
                string value = Elements.GetString(Keys.V);
                return IndexInOptArray(value);
            }
            set
            {
                string key = ValueInOptArray(value);
                Elements.SetString(Keys.V, key);
            }
        }

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
