using System.Diagnostics;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    [DebuggerDisplay("({Value})")]
    public sealed class PdfBoolean : PdfItem
    {
        public PdfBoolean()
        { }

        public PdfBoolean(bool value)
        {
            _value = value;
        }

        public bool Value
        {
            get { return _value; }
        }
        readonly bool _value;

        public static readonly PdfBoolean True = new PdfBoolean(true);

        public static readonly PdfBoolean False = new PdfBoolean(false);

        public override string ToString()
        {
            return _value ? bool.TrueString : bool.FalseString;
        }

        internal override void WriteObject(PdfWriter writer)
        {
            writer.Write(this);
        }
    }
}
