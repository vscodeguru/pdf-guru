namespace PdfSharp.Pdf.Actions
{
    public abstract class PdfAction : PdfDictionary
    {
        protected PdfAction()
        {
            Elements.SetName(Keys.Type, "/Action");
        }

        protected PdfAction(PdfDocument document)
            : base(document)
        {
            Elements.SetName(Keys.Type, "/Action");
        }

        internal class Keys : KeysBase
        {
            [KeyInfo(KeyType.Name | KeyType.Optional, FixedValue = "Action")]
            public const string Type = "/Type";

            [KeyInfo(KeyType.Name | KeyType.Required)]
            public const string S = "/S";

            [KeyInfo(KeyType.ArrayOrDictionary | KeyType.Optional)]
            public const string Next = "/Next";
        }
    }
}
