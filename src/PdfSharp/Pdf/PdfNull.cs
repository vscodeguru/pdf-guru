using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    public sealed class PdfNull : PdfItem
    {
        PdfNull()
        { }

        public override string ToString()
        {
            return "null";
        }

        internal override void WriteObject(PdfWriter writer)
        {
            writer.WriteRaw(" null ");
        }

        public static readonly PdfNull Value = new PdfNull();
    }
}
