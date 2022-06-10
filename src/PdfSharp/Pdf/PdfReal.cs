using System.Diagnostics;
using System.Globalization;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    [DebuggerDisplay("({Value})")]
    public sealed class PdfReal : PdfNumber
    {
        public PdfReal()
        { }

        public PdfReal(double value)
        {
            _value = value;
        }

        public double Value
        {
            get { return _value; }
        }
        readonly double _value;

        public override string ToString()
        {
            return _value.ToString(Config.SignificantFigures3, CultureInfo.InvariantCulture);
        }

        internal override void WriteObject(PdfWriter writer)
        {
            writer.Write(this);
        }
    }
}