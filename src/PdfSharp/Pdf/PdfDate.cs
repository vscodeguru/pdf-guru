using System;
using System.Diagnostics;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    [DebuggerDisplay("({Value})")]
    public sealed class PdfDate : PdfItem
    {
        public PdfDate()
        { }

        public PdfDate(string value)
        {
            _value = Parser.ParseDateTime(value, DateTime.MinValue);
        }

        public PdfDate(DateTime value)
        {
            _value = value;
        }

        public DateTime Value
        {
            get { return _value; }
        }
        DateTime _value;

        public override string ToString()
        {
            string delta = _value.ToString("zzz").Replace(':', '\'');
            return String.Format("D:{0:yyyyMMddHHmmss}{1}'", _value, delta);
        }

        internal override void WriteObject(PdfWriter writer)
        {
            writer.WriteDocString(ToString());
        }
    }
}
