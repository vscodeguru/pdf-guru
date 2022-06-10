
namespace PdfSharp.Pdf.Advanced
{
    public class PdfSoftMask : PdfDictionary
    {
        public PdfSoftMask(PdfDocument document)
            : base(document)
        {
            Elements.SetName(Keys.Type, "/Mask");
        }

        public class Keys : KeysBase
        {
            [KeyInfo(KeyType.Name | KeyType.Optional, FixedValue = "Mask")]
            public const string Type = "/Type";

            [KeyInfo(KeyType.Name | KeyType.Required)]
            public const string S = "/S";

            [KeyInfo(KeyType.Stream | KeyType.Required)]
            public const string G = "/G";

            [KeyInfo(KeyType.Array | KeyType.Optional)]
            public const string BC = "/BC";

            [KeyInfo(KeyType.FunctionOrName | KeyType.Optional)]
            public const string TR = "/TR";
        }
    }
}