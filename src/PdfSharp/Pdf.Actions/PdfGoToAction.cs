namespace PdfSharp.Pdf.Actions
{
    public sealed class PdfGoToAction : PdfAction
    {
        public PdfGoToAction()
        {
            Inititalize();
        }

        public PdfGoToAction(PdfDocument document)
            : base(document)
        {
            Inititalize();
        }

        void Inititalize()
        {
            Elements.SetName(PdfAction.Keys.Type, "/Action");
            Elements.SetName(PdfAction.Keys.S, "/Goto");
        }

        internal new class Keys : PdfAction.Keys
        {
            [KeyInfo(KeyType.Name | KeyType.ByteString | KeyType.Array | KeyType.Required)]
            public const string D = "/D";
        }
    }
}
