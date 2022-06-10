using System.Diagnostics;
using System.Globalization;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    [DebuggerDisplay("({Value})")]
    public sealed class PdfIntegerObject : PdfNumberObject
    {
        public PdfIntegerObject()
        { }

        public PdfIntegerObject(int value)
        {
            _value = value;
        }

        public PdfIntegerObject(PdfDocument document, int value)
            : base(document)
        {
            _value = value;
        }

        public int Value
        {
            get { return _value; }
        }
        readonly int _value;

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
