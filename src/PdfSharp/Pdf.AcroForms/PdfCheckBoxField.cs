using PdfSharp.Pdf.Annotations;
using PdfSharp.Pdf.Advanced;

namespace PdfSharp.Pdf.AcroForms
{
    public sealed class PdfCheckBoxField : PdfButtonField
    {
        internal PdfCheckBoxField(PdfDocument document)
            : base(document)
        {
            _document = document;
        }

        internal PdfCheckBoxField(PdfDictionary dict)
            : base(dict)
        { }

        public string CheckedName
        {
            get { return _checkedName; }
            set { _checkedName = value; }
        }
        string _checkedName = "/Yes";

        public string UncheckedName
        {
            get { return _uncheckedName; }
            set { _uncheckedName = value; }
        }
        string _uncheckedName = "/Off";

        public new class Keys : PdfButtonField.Keys
        {
            [KeyInfo(KeyType.TextString | KeyType.Optional)]
            public const string Opt = "/Opt";

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
