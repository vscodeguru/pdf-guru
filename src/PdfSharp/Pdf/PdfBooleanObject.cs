using System.Diagnostics;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    [DebuggerDisplay("({Value})")]
    public sealed class PdfBooleanObject : PdfObject
    {
        public PdfBooleanObject()
        { }

        public PdfBooleanObject(bool value)
        {
            _value = value;
        }

        public PdfBooleanObject(PdfDocument document, bool value)
            : base(document)
        {
            _value = value;
        }

        public bool Value
        {
            get { return _value; }
        }

        readonly bool _value;

        public override string ToString()
        {
            return _value ? bool.TrueString : bool.FalseString;
        }

        internal override void WriteObject(PdfWriter writer)
        {
            writer.WriteBeginObject(this);
            writer.Write(_value);
            writer.WriteEndObject();
        }
    }
}
