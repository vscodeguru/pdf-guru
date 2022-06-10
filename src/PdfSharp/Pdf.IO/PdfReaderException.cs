using System;

namespace PdfSharp.Pdf.IO
{
    public class PdfReaderException : PdfSharpException
    {
        public PdfReaderException()
        { }

        public PdfReaderException(string message)
            : base(message)
        { }

        public PdfReaderException(string message, Exception innerException)
            :
            base(message, innerException)
        { }
    }
}
