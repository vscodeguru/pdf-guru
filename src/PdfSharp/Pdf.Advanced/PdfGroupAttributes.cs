

namespace PdfSharp.Pdf.Advanced
{
    public abstract class PdfGroupAttributes : PdfDictionary
    {
        internal PdfGroupAttributes(PdfDocument thisDocument)
            : base(thisDocument)
        {
            Elements.SetName(Keys.Type, "/Group");
        }

        public class Keys : KeysBase
        {
            [KeyInfo(KeyType.Name | KeyType.Optional)]
            public const string Type = "/Type";

            [KeyInfo(KeyType.Name | KeyType.Required)]
            public const string S = "/S";

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