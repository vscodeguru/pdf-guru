using System;
using System.Diagnostics;
using PdfSharp.Pdf.IO;

namespace PdfSharp.Pdf
{
    [DebuggerDisplay("({Value})")]
    public sealed class PdfNameObject : PdfObject
    {
        public PdfNameObject()
        {
            _value = "/";    
        }

        public PdfNameObject(PdfDocument document, string value)
            : base(document)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (value.Length == 0 || value[0] != '/')
                throw new ArgumentException(PSSR.NameMustStartWithSlash);

            _value = value;
        }

        public override bool Equals(object obj)
        {
            return _value.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
        string _value;

        public override string ToString()
        {
            return _value;
        }

        public static bool operator ==(PdfNameObject name, string str)
        {
            return name._value == str;
        }

        public static bool operator !=(PdfNameObject name, string str)
        {
            return name._value != str;
        }

        internal override void WriteObject(PdfWriter writer)
        {
            writer.WriteBeginObject(this);
            writer.Write(new PdfName(_value));
            writer.WriteEndObject();
        }
    }
}
