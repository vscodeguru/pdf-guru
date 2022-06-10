using System.Globalization;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    public sealed class PdfRealObject : PdfNumberObject
    {
        public PdfRealObject()
        { }

        public PdfRealObject(double value)
        {
            _value = value;
        }

        public PdfRealObject(PdfDocument document, double value)
            : base(document)
        {
            _value = value;
        }

        public double Value
        {
            get { return _value; }
            set { _value = value; }
        }
        double _value;

        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }

        internal override void WriteObject(PdfWriter writer)
        {
            writer.WriteBeginObject(this);
            writer.Write(_value);
            writer.WriteEndObject();
        }
    }
}
