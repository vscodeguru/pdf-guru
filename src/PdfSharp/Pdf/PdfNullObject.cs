using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    public sealed class PdfNullObject : PdfObject
    {
        public PdfNullObject()
        { }

        public PdfNullObject(PdfDocument document)
            : base(document)
        { }

        public override string ToString()
        {
            return "null";
        }

        internal override void WriteObject(PdfWriter writer)
        {
            writer.WriteBeginObject(this);
            writer.WriteRaw(" null ");
            writer.WriteEndObject();
        }
    }
}
