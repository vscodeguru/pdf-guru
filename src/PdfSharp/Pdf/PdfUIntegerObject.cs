using System.Diagnostics;
using System.Globalization;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    [DebuggerDisplay("({Value})")]
    public sealed class PdfUIntegerObject : PdfNumberObject
    {
        public PdfUIntegerObject()
        { }

        public PdfUIntegerObject(uint value)
        {
            _value = value;
        }

        public PdfUIntegerObject(PdfDocument document, uint value)
            : base(document)
        {
            _value = value;
        }

        public uint Value
        {
            get { return _value; }
        }
        readonly uint _value;

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
