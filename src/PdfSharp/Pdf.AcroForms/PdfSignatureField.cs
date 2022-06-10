namespace PdfSharp.Pdf.AcroForms
{
    public sealed class PdfSignatureField : PdfAcroField
    {
        internal PdfSignatureField(PdfDocument document)
            : base(document)
        { }

        internal PdfSignatureField(PdfDictionary dict)
            : base(dict)
        { }

        public new class Keys : PdfAcroField.Keys
        {
            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string Type = "/Type";

            [KeyInfo(KeyType.Name | KeyType.Required)]
            public const string Filter = "/Filter";

            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string SubFilter = "/SubFilter";

            [KeyInfo(KeyType.Array | KeyType.Required)]
            public const string ByteRange = "/ByteRange";

            [KeyInfo(KeyType.String | KeyType.Required)]
            public const string Contents = "/Contents";

            [KeyInfo(KeyType.TextString | KeyType.Optional)]
            public const string Name = "/Name";

            [KeyInfo(KeyType.Date | KeyType.Optional)]
            public const string M = "/M";

            [KeyInfo(KeyType.TextString | KeyType.Optional)]
            public const string Location = "/Location";

            [KeyInfo(KeyType.TextString | KeyType.Optional)]
            public const string Reason = "/Reason";

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
