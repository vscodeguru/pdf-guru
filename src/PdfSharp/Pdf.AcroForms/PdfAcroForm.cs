namespace PdfSharp.Pdf.AcroForms
{
    public sealed class PdfAcroForm : PdfDictionary
    {
        internal PdfAcroForm(PdfDocument document)
            : base(document)
        {
            _document = document;
        }

        internal PdfAcroForm(PdfDictionary dictionary)
            : base(dictionary)
        { }

        public PdfAcroField.PdfAcroFieldCollection Fields
        {
            get
            {
                if (_fields == null)
                {
                    object o = Elements.GetValue(Keys.Fields, VCF.CreateIndirect);
                    _fields = (PdfAcroField.PdfAcroFieldCollection)o;
                }
                return _fields;
            }
        }
        PdfAcroField.PdfAcroFieldCollection _fields;

        public sealed class Keys : KeysBase
        {
            [KeyInfo(KeyType.Array | KeyType.Required, typeof(PdfAcroField.PdfAcroFieldCollection))]
            public const string Fields = "/Fields";

            [KeyInfo(KeyType.Boolean | KeyType.Optional)]
            public const string NeedAppearances = "/NeedAppearances";

            [KeyInfo("1.3", KeyType.Integer | KeyType.Optional)]
            public const string SigFlags = "/SigFlags";

            [KeyInfo(KeyType.Array)]
            public const string CO = "/CO";

            [KeyInfo(KeyType.Dictionary | KeyType.Optional)]
            public const string DR = "/DR";

            [KeyInfo(KeyType.String | KeyType.Optional)]
            public const string DA = "/DA";

            [KeyInfo(KeyType.Integer | KeyType.Optional)]
            public const string Q = "/Q";

            internal static DictionaryMeta Meta
            {
                get
                {
                    if (s_meta == null)
                        s_meta = CreateMeta(typeof(Keys));
                    return s_meta;
                }
            }
            static DictionaryMeta s_meta;

        }

        internal override DictionaryMeta Meta
        {
            get { return Keys.Meta; }
        }
    }
}
