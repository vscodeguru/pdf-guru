namespace PdfSharp.Pdf.Advanced
{
    public abstract class PdfXObject : PdfDictionary
    {
        protected PdfXObject(PdfDocument document)
            : base(document)
        { }

        public class Keys : PdfStream.Keys
        { }
    }
}
