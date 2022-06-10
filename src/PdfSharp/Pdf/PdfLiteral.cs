using System;
using PdfSharp.Drawing;
using PdfSharp.Pdf.IO;
using PdfSharp.Pdf.Internal;

namespace PdfSharp.Pdf
{
    public sealed class PdfLiteral : PdfItem
    {
        public PdfLiteral()
        { }

        public PdfLiteral(string value)
        {
            _value = value;
        }

        public PdfLiteral(string format, params object[] args)
        {
            _value = PdfEncoders.Format(format, args);
        }

        public static PdfLiteral FromMatrix(XMatrix matrix)
        {
            return new PdfLiteral("[" + PdfEncoders.ToString(matrix) + "]");
        }

        public string Value
        {
            get { return _value; }
        }
        readonly string _value = String.Empty;

        public override string ToString()
        {
            return _value;
        }

        internal override void WriteObject(PdfWriter writer)
        {
            writer.Write(this);
        }
    }
}
